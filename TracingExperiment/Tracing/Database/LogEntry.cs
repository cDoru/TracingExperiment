using System;
using System.Collections.Generic;
using TracingExperiment.Tracing.Database.Interfaces;

namespace TracingExperiment.Tracing.Database
{
    public class LogEntry : IEntity
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime Timestamp { get; set; }

        public virtual DateTime? RequestTimestamp { get; set; }

        public virtual DateTime? ResponseTimestamp { get; set; }

        public virtual string RequestUri { get; set; }

        public virtual ICollection<LogStep> Steps { get; set; } 
    }
}