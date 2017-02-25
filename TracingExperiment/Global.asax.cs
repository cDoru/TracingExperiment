using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using TracingExperiment.App_Start;
using TracingExperiment.Exceptions;
using TracingExperiment.Tracing.Handlers;

namespace TracingExperiment
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.ConfigureContainer();
            GlobalConfiguration.Configuration.MessageHandlers.Add(GetResolver().Resolve<ApiLogHandler>());
        }

        private static ILifetimeScope GetResolver()
        {
            // intiialize the container
            var dependencyComponent = GlobalConfiguration.Configuration.DependencyResolver;
            if (dependencyComponent == null)
                throw new NoDependencyInjectionSetupException("Dependency component not found");

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (!(dependencyComponent is AutofacWebApiDependencyResolver))
            {
                throw new NoAutofacContainerFoundException("Dependency resolver is not Autofac");
            }

            return ((AutofacWebApiDependencyResolver)dependencyComponent).Container;
        }
    }
}
