using System;
using TracingExperiment.Tracing.Database;

namespace TracingExperiment.Tracing.Concurrent
{
    public class TraceStep
    {
        //public virtual Guid Id { get; set; }
        //public virtual DateTime StepTimestamp { get; set; }
        //public virtual StepType Type { get; set; }
        //public virtual string Description { get; set; }
        //public virtual string Metadata { get; set; }

        //public virtual LogEntry LogEntry { get; set; }

        public int Index { get; set; }

        public DateTime StepTimestamp { get; set; }

        public StepType Type { get; set; }

        public string Source { get; set; }

        public string Name { get; set; }

        public string Frame { get; set; }

        public string Message { get; set; }

        public string Metadata { get; set; }
    }
}