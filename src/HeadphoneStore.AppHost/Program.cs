var builder = DistributedApplication.CreateBuilder(args);

// Secret parameters
var sqlServerPassword = builder.AddParameter("SqlServerPassword", secret: true);

// MSSQL database
var sqlServer = builder
    .AddSqlServer("headphonestore-db", port: 5432)
    .WithImage("mssql/server")
    .WithImageTag("2022-latest")
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithEnvironment("MSSQL_SA_PASSWORD", "StrongP@ssw0rd123")
    .WithEnvironment("SA_PASSWORD", "StrongP@ssw0rd123")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataBindMount("../../mnt/mssql")
    .WithDataVolume("sqlserver");

// Redis cache
var redis = builder
    .AddRedis("redis", 6379)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataBindMount("../../mnt/redis")
    .WithRedisCommander();

var apiService = builder
    .AddProject<Projects.HeadphoneStore_API>("api-service")
    .WithReference(sqlServer).WaitFor(sqlServer)
    .WithReference(redis);

var backoffice = builder
    .AddPnpmApp("backoffice", "../HeadphoneStore.Backoffice", "dev")
    .WaitFor(sqlServer)
    .WithHttpEndpoint(env: "PORT", targetPort: 3000)
    .WithEnvironment("BROWSER", "none")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

var storefront = builder
    .AddProject<Projects.HeadphoneStore_StoreFrontEnd>("storefront")
    .WithReference(redis)
    .WaitFor(apiService);

builder.Build().Run();
