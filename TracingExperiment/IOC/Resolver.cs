using System;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using TracingExperiment.Exceptions;
using TracingExperiment.IOC.Interfaces;
using TracingExperiment.Tracing.Utils;

namespace TracingExperiment.IOC
{
    /// <summary>
    /// Resolves any dependency registered in autofac
    /// </summary>
    public class Resolver : IResolver
    {
        private const string ResolverFailure = "Failed to resolve type {0}. More details in the underlying exception.";

        private AutofacWebApiDependencyResolver Container { get; set; }
        private bool _initialized;
        private static readonly object LockObject = new object();

        private void CheckContainer()
        {
            if (_initialized)
            {
                return;
            }

            using (new TimingOutLock(LockObject))
            {
                if (_initialized) return;
                
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
                Container = dependencyComponent as AutofacWebApiDependencyResolver;
                _initialized = true;
            }
        }
    
        /// <summary>
        /// Resolves simple dependency based on the autofac di container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            CheckContainer();

            try
            {
                return Container.Container.Resolve<T>();
            }
            catch(Exception exception)
            {
                throw new ResolverException(string.Format(ResolverFailure, typeof (T).FullName), exception);
            }
        }
    }
}