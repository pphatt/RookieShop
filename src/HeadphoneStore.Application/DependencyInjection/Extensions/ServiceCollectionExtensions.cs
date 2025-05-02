using FluentValidation;
using FluentValidation.AspNetCore;

using HeadphoneStore.Application.Behaviors;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace HeadphoneStore.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRApplication(this IServiceCollection services)
    {
        // previous version of "MediatR.Extensions.Microsoft.DependencyInjection" is deprecated and not needed from v12 of MediatR.
        // old school way (previous of the v12 need to install "MediatR.Extensions.Microsoft.DependencyInjection" package):
        // services.AddMediatR(typeof(DependencyInjection).Assembly); -> MediatR v11.1.0
        // https://stackoverflow.com/a/72263414
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingPipelineBehavior<,>));

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly)
            .AddFluentValidationAutoValidation();

        return services;
    }
}
