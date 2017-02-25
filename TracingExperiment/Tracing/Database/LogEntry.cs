using System;
using TracingExperiment.Tracing.Database.Interfaces;

namespace TracingExperiment.Tracing.Database
{
    public class LogEntry : IEntity
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public DateTime? RequestTimestamp { get; set; }

        public DateTime? ResponseTimestamp { get; set; }

        public string TraceData { get; set; }

        public string RequestUri { get; set; }
    }
}