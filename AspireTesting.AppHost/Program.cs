var builder = DistributedApplication.CreateBuilder(args);

var databaseConfiguration = builder.AddPostgres(name: "database", port: 5433)
    .WithEnvironment("POSTGRES_DB", "Testing");

databaseConfiguration = SetupData(args, databaseConfiguration);

var database = databaseConfiguration.AddDatabase(name: "AppDatabase", databaseName: "Testing");

builder.AddContainer("Mail", "mailhog/mailhog")
    .WithEndpoint(port: 8025, targetPort: 8025, name: "web", scheme: "http")
    .WithEndpoint(port: 1025, targetPort: 1025, name: "smtp");

builder.AddProject<Projects.AspireTesting>(Constants.ApiRespourceName)
    .WithReference(database);

builder.Build().Run();

IResourceBuilder<PostgresServerResource> SetupData(string[] args, 
    IResourceBuilder<PostgresServerResource> databaseConfiguration)
{
    if (args.Any(arg => arg == Constants.IntegrationTest))
    {
        databaseConfiguration = databaseConfiguration
            .WithBindMount("./data", "/docker-entrypoint-initdb.d");
    }
    else
    {
        databaseConfiguration = databaseConfiguration.WithDataVolume();
    }

    return databaseConfiguration;
}

public class Constants 
{
    public const string ApiRespourceName = "aspire-testing";

    public const string IntegrationTest = "IntegrationTest";
}