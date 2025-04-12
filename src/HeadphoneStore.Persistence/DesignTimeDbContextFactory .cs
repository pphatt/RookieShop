using HeadphoneStore.Persistence.DependencyInjection.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HeadphoneStore.Persistence;

// The DesignTimeDbContextFactory is only used during design time operations (like migrations) when the application isn't running.
// It's not part of the runtime application configuration.
// And AddSqlServerPersistence method is used when your application is running to configure services in the dependency injection container.
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var retryOptions = configuration.GetSection("SqlServerRetryOptions").Get<SqlServerRetryOptions>();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder
            .EnableDetailedErrors(true)
            .EnableSensitiveDataLogging(true)
            .UseLazyLoadingProxies(true) // If using UseLazyLoadingProxies, all of the navigation fields (entities declaration) should be VIRTUAL
            .UseSqlServer(
                connectionString,
                sqlServerOptions =>
                    sqlServerOptions
                        .ExecutionStrategy(dependencies => new SqlServerRetryingExecutionStrategy(
                            dependencies,
                            maxRetryCount: retryOptions.MaxRetryCount,
                            maxRetryDelay: retryOptions.MaxRetryDelay,
                            errorNumbersToAdd: retryOptions.ErrorNumbersToAdd))
                        .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)
                        .EnableRetryOnFailure());

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
