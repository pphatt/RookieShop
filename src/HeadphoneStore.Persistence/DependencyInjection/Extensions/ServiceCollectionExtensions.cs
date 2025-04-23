using HeadphoneStore.API.Interceptors;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Persistence.DependencyInjection.Options;
using HeadphoneStore.Persistence.Interceptors;
using HeadphoneStore.Persistence.Repositories;
using HeadphoneStore.Persistence.Repository;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HeadphoneStore.Persistence.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSqlServerPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<ApplicationDbContext>((provider, builder) =>
        {
            var auditableInterceptor = provider.GetService<UpdateAuditableEntitiesInterceptor>();
            var deletableInterceptor = provider.GetService<DeleteAuditableEntitiesInterceptor>();

            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();

            builder
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
                .UseLazyLoadingProxies(true) // If using UseLazyLoadingProxies, all of the navigation fields (entities declaration) should be VIRTUAL
                .UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: optionsBuilder =>
                        optionsBuilder
                            .ExecutionStrategy(dependencies => new SqlServerRetryingExecutionStrategy(
                                dependencies,
                                maxRetryCount: options.CurrentValue.MaxRetryCount,
                                maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd))
                            .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)
                            .EnableRetryOnFailure());
        });
    }

    public static OptionsBuilder<SqlServerRetryOptions> ConfigureSqlServerRetryOptionsPersistence(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions<SqlServerRetryOptions>()
            .Bind(configuration.GetSection(nameof(SqlServerRetryOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

    public static void AddRepositoryPersistence(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));

        var concreteServices = typeof(CategoryRepository).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(i => i.Name == typeof(IRepositoryBase<,>).Name)
                && !x.IsAbstract
                && x.IsClass
                && !x.IsGenericType)
            .OrderBy(x => x.Name.Contains("Cache") ? 1 : 0);

        foreach (var concreteService in concreteServices)
        {
            var allInterfaces = concreteService.GetInterfaces();

            var inheritedInterface = allInterfaces.SelectMany(x => x.GetInterfaces());

            var directInterface = allInterfaces.Except(inheritedInterface).FirstOrDefault();

            if (directInterface != null)
            {
                services.Add(new ServiceDescriptor(directInterface, concreteService, ServiceLifetime.Scoped));
            }
        }
    }

    public static void AddDbIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(options =>
            // settings for all 4 tokens in TokenProviders not just Reset Password Token.
            options.TokenLifespan = TimeSpan.FromMinutes(30)
        );

        services.Configure<IdentityOptions>(options =>
        {
            // Email settings.
            options.SignIn.RequireConfirmedEmail = true;

            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        });
    }
}
