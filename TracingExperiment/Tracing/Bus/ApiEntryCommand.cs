using System.Collections.Generic;
using Enexure.MicroBus;
using TracingExperiment.Tracing.Concurrent;
using TracingExperiment.Tracing.Handlers;

namespace TracingExperiment.Tracing.Bus
{
    public class ApiEntryCommand : ICommand
    {
        public ApiLogEntry Entry { get; private set; }
        public List<TraceStep> Steps { get; private set; }

        public ApiEntryCommand(ApiLogEntry entry, List<TraceStep> steps)
        {
            Entry = entry;
            Steps = steps;
        }
    }
}