
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using TracingExperiment.Exceptions;
using TracingExperiment.Tracing.Database.Configurations;
using TracingExperiment.Tracing.Database.Interfaces;

namespace TracingExperiment.Tracing.Database
{
    public class TracingContext : DbContext, ITracingContext
    {
        public TracingContext() : this(WebConfigurationManager.AppSettings.Get("ConnectionString"))
        {
            
        }

        public TracingContext(string connectionString)
            : base(connectionString)
        {
        }

        public TracingContext(string connectionString, DbCompiledModel model)
            : base(connectionString, model)
        {
        }

        public TracingContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        public TracingContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new LogEntryConfiguration());
        }

        public void Attach<T>(T entity) where T : class, IEntity
        {
            Set<T>().Attach(entity);
        }

        public IQueryable<T> AsQueryable<T>() where T : class, IEntity
        {
            return Set<T>().AsQueryable();
        }

        public void Update<T>(T entity) where T : class, IEntity
        {
            EnsureAttachedEf(entity).State = EntityState.Modified;
            var orig = Set<T>().Find(entity.Id);
            if (orig != null)
            {
                Entry(entity).CurrentValues.SetValues(entity);
            }

            SaveChanges();
        }

        public void Save<T>(T entity) where T : class, IEntity
        {
            Set<T>().Add(entity);
            SaveChanges();
        }

        DbEntityEntry<T> EnsureAttachedEf<T>(T entity) where T : class, IEntity
        {
            if (Entry(entity).State == EntityState.Detached)
                Set<T>().Attach(entity);

            return Entry(entity);
        }

        public override int SaveChanges()
        {
            //foreach ( var ent in from entity in ChangeTracker.Entries() 
            //                     let ent = entity.Entity as IEntity 
            //                     where ent != null 
            //                     where entity.State == EntityState.Detached 
            //                     select ent )
            //{
            //    Attach(ent);
            //}

            return ExecuteDatabaseSave(base.SaveChanges);
        }

        static int ExecuteDatabaseSave(Func<int> function)
        {
            try
            {
                return function();
            }
            catch (DbEntityValidationException dbEx)
            {
                var exception = HandleEntityValidationException(dbEx);
                throw exception;
            }
            catch (DbUpdateException dbUex)
            {
                var exception = HandleDbUpdateException(dbUex);
                throw exception;
            }
            catch (ConstraintException ce)
            {
                var exception = HandleConstraintException(ce);
                throw exception;
            }
            catch (Exception ex)
            {
                throw new TracingContextException(ex.Message);
            }
        }


        private static Exception HandleConstraintException(ConstraintException ce)
        {
            var outputLines = new StringBuilder();

            outputLines.AppendLine("The following constraints were violated");
            foreach (KeyValuePair<string, string> entry in ce.Data)
            {
                outputLines.AppendLine(" Key: " + entry.Key + " , Value: " + entry.Value);
            }

            var detailedException = outputLines.ToString();

            return new TracingContextException(detailedException);
        }

        private static Exception HandleDbUpdateException(DbUpdateException dbUex)
        {
            Exception result;
            // let the unique constraint exceptions flow 
            var innerException = dbUex.InnerException == null
                ? null
                : dbUex.InnerException.InnerException as SqlException;

            if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
            {
                //your handling stuff
                IEnumerable<DbEntityEntry> entries = dbUex.Entries.ToList();

                var outputLines = new StringBuilder();

                if (entries.Count() > 1)
                {
                    outputLines.AppendLine(string.Format("There are {0} entries conflicting :", entries.Count()));
                }

                var index = 1;

                foreach (var entity in entries)
                {
                    outputLines.AppendLine(string.Format("conflicting entry {0}", index));
                    //outputLines.AppendLine(ObjectDumper.Dump(entity.Entity));
                    outputLines.AppendLine(string.Format("end of conflicting entry {0}", index));

                    index++;
                }

                var detailedException = outputLines.ToString();

                result = new TracingContextUniqueConstraintFailureException(detailedException);
            }
            else
                if (!(dbUex.InnerException is UpdateException) ||
                    !(dbUex.InnerException.InnerException is SqlException))
                {
                    IEnumerable<DbEntityEntry> entries = dbUex.Entries.ToList();

                    var outputLines = new StringBuilder();

                    if (entries.Count() > 1)
                    {
                        outputLines.AppendLine(string.Format("There are {0} entries conflicting :", entries.Count()));
                    }

                    var index = 1;

                    foreach (var entity in entries)
                    {
                        outputLines.AppendLine(string.Format("conflicting entry {0}", index));
                        //outputLines.AppendLine(entity.);
                        outputLines.AppendLine(string.Format("end of conflicting entry {0}", index));

                        index++;
                    }

                    var detailedException = outputLines.ToString();
                    result = new TracingContextException(detailedException);
                }
                else
                {
                    var sqlException =
                        (SqlException)dbUex.InnerException.InnerException;

                    var outputLines = new StringBuilder();
                    for (var i = 0; i < sqlException.Errors.Count; i++)
                    {
                        var errorNum = sqlException.Errors[i].Number;
                        string errorText;
                        if (SqlErrorTextDict.TryGetValue(errorNum, out errorText))
                            outputLines.AppendLine(errorText);
                    }

                    var detailedException = outputLines.ToString();
                    result = new TracingContextException(detailedException);
                }
            return result;
        }

        private static readonly Dictionary<int, string> SqlErrorTextDict =
              new Dictionary<int, string>
            {
                {547,
                 "This operation failed because another data entry uses this entry."},        
                {2601,
                 "One of the properties is marked as Unique index and there is already an entry with that value."}
            };

        private static Exception HandleEntityValidationException(DbEntityValidationException dbEx)
        {
            var outputLines = new StringBuilder();
            foreach (var eve in dbEx.EntityValidationErrors)
            {
                outputLines.AppendLine(
                    string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                        DateTime.UtcNow, eve.Entry.Entity.GetType().Name, eve.Entry.State));

                foreach (var ve in eve.ValidationErrors)
                {
                    outputLines.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                }
            }

            var detailedException = outputLines.ToString();

            return new TracingContextException(detailedException);
        }
    }
}