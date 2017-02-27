using System.Collections.Generic;
using TracingExperiment.Tracing.Concurrent;

namespace TracingExperiment.Tracing.Interfaces
{
    public interface ITracer
    {
        void WriteMessage(string source, string frame, string message);
        void WriteException(string source, string frame, string exception, string description, string name);

        void WriteOperation(string source, string frame, 
            string description, string name, string operationMetadata);
        List<TraceStep> TraceSteps { get; } 
    }
}