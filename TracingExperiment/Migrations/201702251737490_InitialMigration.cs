namespace TracingExperiment.Tracing.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        RequestTimestamp = c.DateTime(),
                        ResponseTimestamp = c.DateTime(),
                        TraceData = c.String(nullable: false, storeType: "ntext"),
                        RequestUri = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogEntries");
        }
    }
}
