﻿using System.Web.Configuration;
using TracingExperiment.Tracing.Interfaces;

namespace TracingExperiment.Tracing.WCF
{
    public class WcfHelper : IHelper
    {
        private const string Key = "log-workflow";

        public WcfHelper()
        {
            InitializeShouldLog();
        }

        private void InitializeShouldLog()
        {
            bool log;
            ShouldLog = bool.TryParse(WebConfigurationManager.AppSettings[Key], out log) && log;
        }

        public bool ShouldLog { get; private set; }
    }
}