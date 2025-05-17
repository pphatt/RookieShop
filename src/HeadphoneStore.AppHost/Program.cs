using Google.Protobuf.WellKnownTypes;

var builder = DistributedApplication.CreateBuilder(args);

// Secret parameters
var sqlServerPassword = builder.AddParameter("SqlServerPassword", secret: true);

// MSSQL database
var sqlServer = builder
    .AddSqlServer("database", sqlServerPassword)
    .WithImage("mssql/server")
    .WithImageTag("2022-latest")
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithEnvironment("MSSQL_SA_PASSWORD", sqlServerPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataBindMount(source: "../../mnt/mssql")
    .WithDataVolume("sqlserver", isReadOnly: false);

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

builder.AddProject<Projects.HeadphoneStore_Proxy>("proxy")
    .WithHttpsEndpoint(8080, name: "proxy", isProxied: false)
    .WithReference(apiService)
    .WithReference(backoffice)
    .WithReference(storefront);

builder.Build().Run();
