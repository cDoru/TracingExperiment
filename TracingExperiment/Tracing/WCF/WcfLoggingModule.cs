using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Autofac;
using TracingExperiment.Tracing.Interfaces;

namespace TracingExperiment.Tracing.WCF
{
    /// <summary>
    /// SoapRequestAndResponseTracingIocModule class
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class WcfLoggingModule : Module
    {
        /// <summary>
        /// Load method - SoapRequestAndResponseTracingIocModule's override of Module's Load method
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WcfHelper>().As<IHelper>().SingleInstance();
            builder.RegisterType<TracingMessageInspector>()
                .As<IClientMessageInspector>().InstancePerLifetimeScope();

            builder.RegisterType<TracingMessageDispatcher>()
                .As<IDispatchMessageInspector>().InstancePerLifetimeScope();

            builder.RegisterType<TracingMessageBehavior>()
                .As<IEndpointBehavior>().InstancePerLifetimeScope();
        }

    }
}