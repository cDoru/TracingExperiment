using System;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
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
        private readonly ILifetimeScope _scope;
        private const string ResolverFailure = "Failed to resolve type {0}. More details in the underlying exception.";

        public Resolver(ILifetimeScope scope)
        {
            _scope = scope;
        }
        /// <summary>
        /// Resolves simple dependency based on the autofac di container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            try
            {
                return _scope.Resolve<T>();
            }
            catch(Exception exception)
            {
                throw new ResolverException(string.Format(ResolverFailure, typeof (T).FullName), exception);
            }
        }
    }
}