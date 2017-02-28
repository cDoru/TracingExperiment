using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using TracingExperiment.Exceptions;
using TracingExperiment.Tracing.Interfaces;

namespace TracingExperiment.Tracing.Utils
{
    public interface ITraceStepUtil
    {
        ITraceStepper Get();
    }


    public class TraceStepUtil : ITraceStepUtil
    {
        public ITraceStepper Get()
        {
            var container = FetchContainer();

            return container.Resolve<ITraceStepper>();
        }

        private static readonly object LockObject = new object();

        private static ILifetimeScope FetchContainer()
        {
            using (new TimingOutLock(LockObject))
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

                // ReSharper disable once TryCastAlwaysSucceeds
                // ReSharper disable once SuspiciousTypeConversion.Global
                return (dependencyComponent as AutofacWebApiDependencyResolver).Container;
            }
        }
    }
}