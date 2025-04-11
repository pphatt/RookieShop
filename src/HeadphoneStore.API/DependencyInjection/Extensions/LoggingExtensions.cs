using Serilog;

namespace HeadphoneStore.API.DependencyInjection.Extensions;

public static class LoggingExtensions
{
    public static void AddSerilog(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
    }

    public static void AddLogging(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });
    }
}
