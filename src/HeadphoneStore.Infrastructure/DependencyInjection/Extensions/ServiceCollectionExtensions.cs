using HeadphoneStore.Application.Abstracts.Interface;
using HeadphoneStore.Infrastructure.Services.Authentication;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeadphoneStore.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDependenciesLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBusinessServices();

        return services;
    }

    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IClaimsTransformation, ClaimsTransformer>();
        services.AddScoped<IPermissionService, PermissionService>();

        return services;
    }
}
