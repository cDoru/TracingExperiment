using Microsoft.Owin;
using Owin;
using TracingExperiment;

[assembly: OwinStartup(typeof(Startup))]

namespace TracingExperiment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
