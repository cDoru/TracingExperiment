using System;
using System.Threading.Tasks;
using Enexure.MicroBus;
using TracingExperiment.Tracing.Database;
using TracingExperiment.Tracing.Database.Interfaces;
using TracingExperiment.Tracing.Utils.Interfaces;

namespace TracingExperiment.Tracing.Bus
{
    public class ApiEntryHandler : ICommandHandler<ApiEntryCommand>
    {
        private readonly ITracingContext _context;
        private readonly INow _now;

        public ApiEntryHandler(ITracingContext context, INow now)
        {
            _context = context;
            _now = now;
        }

        public Task Handle(ApiEntryCommand command)
        {
            _context.Save(new LogEntry
            {
                Id = Guid.NewGuid(),
                RequestTimestamp = command.Entry.RequestTimestamp,
                RequestUri = command.Entry.RequestUri,
                ResponseTimestamp = command.Entry.ResponseTimestamp,
                Timestamp = _now.UtcNow, // hide this under an interface
                TraceData = command.Trace
            });

            return Task.FromResult(0);
        }
    }
}