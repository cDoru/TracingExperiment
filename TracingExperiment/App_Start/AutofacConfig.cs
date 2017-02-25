using System.Linq;
using System.Reflection;
using System.ServiceModel.Description;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.WebApi;
using TracingExperiment.IOC;
using TracingExperiment.IOC.Interfaces;
using TracingExperiment.Tracing;
using TracingExperiment.Tracing.Handlers;
using TracingExperiment.Tracing.Interfaces;

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
            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacResolver(Container));
            GlobalConfiguration.Configuration.DependencyResolver =
                 new AutofacWebApiDependencyResolver(Container);
        }

        private static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<SimpleTracer>().As<ITracer>().InstancePerLifetimeScope();
            builder.RegisterType<TraceStepper>().As<ITraceStepper>();

            builder.RegisterType<ApiLogHandler>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<Resolver>().As<IResolver>().SingleInstance();
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