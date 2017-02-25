using System;

namespace TracingExperiment.Tracing.Utils.Interfaces
{
    public interface INow
    {
        DateTime Now { get;}
        DateTime UtcNow { get;}
    }
}
