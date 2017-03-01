using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enexure.MicroBus;
using TracingExperiment.Tracing.Database;
using TracingExperiment.Tracing.Database.Interfaces;
using TracingExperiment.Tracing.Interfaces;
using TracingExperiment.Tracing.Utils.Interfaces;

namespace TracingExperiment.Tracing.Bus
{
    public class ApiEntryHandler : ICommandHandler<ApiEntryCommand>
    {
        private readonly ITracingContext _context;
        private readonly INow _now;
        private readonly IHelper _helper;

        public ApiEntryHandler(ITracingContext context, INow now, IHelper helper)
        {
            _context = context;
            _now = now;
            _helper = helper;
        }

        public Task Handle(ApiEntryCommand command)
        {
            if (_helper.ShouldLog)
            {
                var logEntry = new LogEntry
                {
                    Id = Guid.NewGuid(),
                    Timestamp = _now.UtcNow,
                    RequestTimestamp = command.Entry.RequestTimestamp,
                    ResponseTimestamp = command.Entry.ResponseTimestamp,
                    RequestUri = command.Entry.RequestUri,
                    Steps = new List<LogStep>()
                };

                foreach (var logStep in command.Steps.OrderBy(x => x.Index).ToList().Select(step => new LogStep
                {
                    Id = Guid.NewGuid(),
                    LogEntry = logEntry,
                    LogEntryId = logEntry.Id,
                    Index = step.Index,
                    Metadata = step.Metadata,
                    StepTimestamp = step.StepTimestamp,
                    Type = step.Type,
                    Frame = step.Frame,
                    Name = step.Name,
                    Message = step.Message,
                    Source = step.Source,
                }))
                {
                    logEntry.Steps.Add(logStep);
                }

                _context.Save(logEntry);
            }

            return Task.FromResult(0);
        }
    }
}