using System;
using System.Threading.Tasks;
using Enexure.MicroBus;
using TracingExperiment.Tracing.Database;
using TracingExperiment.Tracing.Database.Interfaces;

namespace TracingExperiment.Tracing.Bus
{
    public class ApiEntryHandler : ICommandHandler<ApiEntryCommand>
    {
        private readonly ITracingContext _context;

        public ApiEntryHandler(ITracingContext context)
        {
            _context = context;
        }

        public Task Handle(ApiEntryCommand command)
        {
            _context.Save(new LogEntry
            {
                Id = Guid.NewGuid(),
                RequestTimestamp = command.Entry.RequestTimestamp,
                RequestUri = command.Entry.RequestUri,
                ResponseTimestamp = command.Entry.ResponseTimestamp,
                Timestamp = DateTime.UtcNow, // hide this under an interface
                TraceData = command.Trace
            });

            return Task.FromResult(0);
        }
    }
}