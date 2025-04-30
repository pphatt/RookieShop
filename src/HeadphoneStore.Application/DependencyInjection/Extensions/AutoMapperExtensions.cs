using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HeadphoneStore.Application.DependencyInjection.Extensions;

public static class AutoMapperExtensions
{
    public static IServiceCollection AddAutoMapperApplication(this IServiceCollection services)
    {
        // Add auto-mapper service.
        services.AddAutoMapper(AssemblyReference.Assembly);

        return services;
    }

    public static WebApplication AddAutoMapperValidationApplication(this WebApplication app)
    {
        //var scope = app.Services.CreateScope();
        //var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        //mapper.ConfigurationProvider.AssertConfigurationIsValid();

        return app;
    }
}
