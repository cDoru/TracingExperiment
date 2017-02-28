using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using TracingExperiment.IOC;
using TracingExperiment.IOC.Interfaces;
using TracingExperiment.Tracing.Bus;
using TracingExperiment.Tracing.Concurrent;
using TracingExperiment.Tracing.Database;
using TracingExperiment.Tracing.Database.Interfaces;
using TracingExperiment.Tracing.Handlers;
using TracingExperiment.Tracing.Interfaces;
using TracingExperiment.Tracing.Utils;
using TracingExperiment.Tracing.Utils.Interfaces;

namespace TracingExperiment.App_Start
{
    public class AutofacConfig
    {
        private class Inner { }

        private static IContainer Container { get; set; }

        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(AutofacConfig).Assembly);

            RegisterDependencies(builder);
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            RegisterModules(builder, assemblies);
            AutowireProperties(builder);
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule(new BusDomainModule());
            builder.Register<IResolver>(x => new Resolver(Container));

            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacResolver(Container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }

        private static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<SimpleTracer>().As<ITracer>().InstancePerLifetimeScope();
            builder.RegisterType<TraceStepper>().As<ITraceStepper>().InstancePerLifetimeScope();

            builder.RegisterType<ApiLogHandler>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<Resolver>().As<IResolver>().SingleInstance();

            builder.RegisterType<NowImplementation>().As<INow>().InstancePerLifetimeScope();
            builder.RegisterType<TracingContext>().As<ITracingContext>().InstancePerLifetimeScope();
            builder.RegisterType<TraceStepUtil>().As<ITraceStepUtil>().InstancePerLifetimeScope();
        }

        private static void AutowireProperties(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(typeof(Inner).Assembly)
                .PropertiesAutowired();

            builder.RegisterType<WebApiApplication>()
                .PropertiesAutowired();
        }

        private static void RegisterModules(ContainerBuilder builder, Assembly[] assemblies)
        {
            // register modules from assemblies
            builder.RegisterAssemblyModules(assemblies);
        }
    }
}