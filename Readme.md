##What this is

This is a playground for a POC tracer which logs incoming/outgoing activity for web api but also allows you to log in any raw request/response coming from a wcf client.
The traces is very simple and it does not (yet) allow imbricated traces.

##What this is not

It's more of a demo project so use at your own risk in production

It also showcases protecting the web.config db connection string with the rsa protection cipher. 

##Useful

This adds an initial migration to the project (migration was already added but just in case)
Add-Migration -Verbose -ConfigurationTypeName TracingConfiguration -ConnectionString "Data Source=.;Initial Catalog=Tracing;Integrated Security=True;"  -ConnectionProviderName "System.Data.SqlClient" InitialMigration


##Usage 

Run the script found inside App_Data (obtained by running Update-Database -Script -ConfigurationTypeName TracingConfiguration -Verbose -ConnectionString "Data Source=.;Initial Catalog=Tracing;Integrated Security=True;" -ConnectionProviderName "System.Data.SqlClient")
The db name should be Tracing

Start the project
