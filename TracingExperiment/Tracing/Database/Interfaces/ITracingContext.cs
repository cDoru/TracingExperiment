using System;
using System.Data.Entity;
using System.Linq;

namespace TracingExperiment.Tracing.Database.Interfaces
{
    public interface ITracingContext : IDisposable
    {
        void Attach<T>(T entity) where T : class, IEntity;
        IQueryable<T> AsQueryable<T>() where T : class, IEntity;
        void Update<T>(T entity) where T : class, IEntity;
        void Save<T>(T entity) where T : class, IEntity;


        DbSet<LogEntry> LogEntries { get; set; }
    }
}