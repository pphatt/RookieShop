using System.Threading.RateLimiting;

using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

// YARP - Configure RateLimiter Service to prevent DoS attacks
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("rateLimitPolicy", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromSeconds(15);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseRateLimiter();

app.MapReverseProxy();

await app.RunAsync();
