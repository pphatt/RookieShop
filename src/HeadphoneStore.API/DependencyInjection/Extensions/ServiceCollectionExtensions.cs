using HeadphoneStore.API.Authorization;

using Microsoft.AspNetCore.Authorization;

using Server.Api.Authorization;

namespace HeadphoneStore.API.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureCors(
        this IServiceCollection services,
        IConfiguration configuration,
        string serverCorsPolicy)
    {
        services.AddCors(p => p.AddPolicy(serverCorsPolicy, builderCors =>
        {
            var origins = configuration["AllowedOrigins"];

            builderCors
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins(origins!);
        }));
    }
}
