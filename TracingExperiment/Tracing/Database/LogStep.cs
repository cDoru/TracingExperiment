using System;
using TracingExperiment.Tracing.Database.Interfaces;

namespace TracingExperiment.Tracing.Database
{
    public class LogStep : IEntity
    {
        public virtual Guid Id { get; set; }
        public virtual int Index { get; set; }
        public virtual DateTime StepTimestamp { get; set; }
        public virtual StepType Type { get; set; }
        public virtual string Source { get; set; }
        public virtual string Name { get; set; }
        public virtual string Frame { get; set; }
        public virtual string Metadata { get; set; }
        public virtual string Message { get; set; }
        public virtual LogEntry LogEntry { get; set; }
    }
}