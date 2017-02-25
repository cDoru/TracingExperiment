using System.Web.Configuration;
using TracingExperiment.Tracing.Interfaces;

namespace TracingExperiment.Tracing.WCF
{
    public class WcfHelper : IHelper
    {
        private const string Key = "log-workflow";

        public bool ShouldLog()
        {
            bool log;
            return bool.TryParse(WebConfigurationManager.AppSettings[Key], out log) && log;
        }
    }
}