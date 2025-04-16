using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Application.Abstracts.Interface.Services.Mail;
using HeadphoneStore.Infrastructure.DependencyInjection.Options;
using HeadphoneStore.Infrastructure.Services.Authentication;
using HeadphoneStore.Infrastructure.Services.Mail;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HeadphoneStore.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDependenciesLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBusinessServices();

        services.AddSingleton<IEmailService, EmailService>();

        return services;
    }

    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IClaimsTransformation, ClaimsTransformer>();
        services.AddScoped<IPermissionService, PermissionService>();

        return services;
    }

    public static OptionsBuilder<EmailOption> ConfigureEmailOptionsInfrastructure(this IServiceCollection services, IConfigurationSection section)
        => services
            .AddOptions<EmailOption>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
}
