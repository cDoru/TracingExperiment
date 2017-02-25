using System;
using TracingExperiment.Tracing.Utils.Interfaces;

namespace TracingExperiment.Tracing.Utils
{
    public class NowImplementation : INow
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}