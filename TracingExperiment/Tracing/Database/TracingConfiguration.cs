using System.Data.Entity.Migrations;

namespace TracingExperiment.Tracing.Database
{
    internal sealed class TracingConfiguration : DbMigrationsConfiguration<TracingContext>
    {
        public TracingConfiguration()
        {
            // so that entityframework doesn't cry
            AutomaticMigrationsEnabled = false;
        }
    }
}