using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Application.Abstracts.Interface.Services.Datetime;
using HeadphoneStore.Application.Abstracts.Interface.Services.Identity;
using HeadphoneStore.Application.Abstracts.Interface.Services.Mail;
using HeadphoneStore.Application.Abstracts.Interface.Services.Media;
using HeadphoneStore.Infrastructure.DependencyInjection.Options;
using HeadphoneStore.Infrastructure.Services.Authentication;
using HeadphoneStore.Infrastructure.Services.Cloudinary;
using HeadphoneStore.Infrastructure.Services.Datetime;
using HeadphoneStore.Infrastructure.Services.Identity;
using HeadphoneStore.Infrastructure.Services.Mail;

using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HeadphoneStore.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDependenciesLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBusinessServices();

        services.AddHttpContextAccessor();

        services
            .AddSingleton<IEmailService, EmailService>()
            .AddSingleton<ICloudinaryService, CloudinaryService>()
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddScoped<IIdentityService, IdentityService>();

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

    public static OptionsBuilder<MediaOption> ConfigureMediaOptionsInfrastructure(this IServiceCollection services, IConfigurationSection section)
    => services
        .AddOptions<MediaOption>()
        .Bind(section)
        .ValidateDataAnnotations()
        .ValidateOnStart();

    public static OptionsBuilder<CloudinaryOption> ConfigureCloudinaryOptionsInfrastructure(this IServiceCollection services, IConfigurationSection section)
        => services
            .AddOptions<CloudinaryOption>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
}
