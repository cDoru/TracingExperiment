using Enexure.MicroBus;
using TracingExperiment.Tracing.Handlers;

namespace TracingExperiment.Tracing.Bus
{
    public class ApiEntryCommand : ICommand
    {
        public ApiLogEntry Entry { get; private set; }
        public string Trace { get; private set; }

        public ApiEntryCommand(ApiLogEntry entry, string trace)
        {
            Entry = entry;
            Trace = trace;
        }
    }
}