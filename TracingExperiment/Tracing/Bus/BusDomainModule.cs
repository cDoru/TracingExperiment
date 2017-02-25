using Autofac;
using Enexure.MicroBus;
using Enexure.MicroBus.Autofac;

namespace TracingExperiment.Tracing.Bus
{
    public class BusDomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var busBuilder = new BusBuilder();
            busBuilder.RegisterCommandHandler<ApiEntryCommand, ApiEntryHandler>();
            builder.RegisterMicroBus(busBuilder);
            base.Load(builder);
        }
    }
}