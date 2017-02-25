using System.Threading.Tasks;
using Enexure.MicroBus;

namespace TracingExperiment.Tracing.Bus
{
    public class ApiEntryHandler : ICommandHandler<ApiEntryCommand>
    {
        public Task Handle(ApiEntryCommand command)
        {


            return Task.FromResult(0);
        }
    }
}