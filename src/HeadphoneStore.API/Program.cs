using System.Text.Json;
using System.Text.Json.Serialization;

using Asp.Versioning;

using HeadphoneStore.API;
using HeadphoneStore.API.DependencyInjection.Extensions;
using HeadphoneStore.API.Middlewares;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Infrastructure.DependencyInjection.Extensions;
using HeadphoneStore.Infrastructure.DependencyInjection.Options;
using HeadphoneStore.Persistence.DependencyInjection.Extensions;

using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

using TreeCommerce.API.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

var serverCorsPolicy = "ServerCorsPolicy";

// Add Controller
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    })
    .AddApplicationPart(AssemblyReference.Assembly);

// Add Api Versioning
builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

// Add Cors
builder.Services.ConfigureCors(builder.Configuration, serverCorsPolicy);

// Add SeriLog
builder.Host.AddLogging();

// Add Swagger
builder.Services
    .AddSwaggerGenNewtonsoftSupport()
    .AddFluentValidationRulesToSwagger()
    .AddEndpointsApiExplorer()
    .AddSwaggerAPI();

// Application Layer
builder.Services.AddMediatRApplication();

// Infrastructure Layer
builder.Services.AddInfrastructureDependenciesLayer(builder.Configuration);
builder.Services.ConfigureEmailOptionsInfrastructure(builder.Configuration.GetSection(nameof(EmailOption)));
builder.Services.ConfigureMediaOptionsInfrastructure(builder.Configuration.GetSection(nameof(MediaOption)));
builder.Services.ConfigureCloudinaryOptionsInfrastructure(builder.Configuration.GetSection(nameof(CloudinaryOption)));
builder.Services.ConfigureCacheOptionsInfrastructure(builder.Configuration.GetSection(nameof(CacheOption)));
builder.Services.AddHttpContextAccessor();

// Persistence Layer
builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration);
builder.Services.AddSqlServerPersistence(builder.Configuration);
builder.Services.AddDbIdentity();
builder.Services.AddRepositoryPersistence();

// API Layer
builder.Services.AddJwtAuthentication(builder.Configuration); // authentication and authorization

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

var app = builder.Build();

// Using middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.AddSwaggerUI();

    app.AddMigration();
}

// Add SeriLog
app.AddSerilog();

// Add Cors
app.UseCors(serverCorsPolicy);

// Add Core
app.UseRouting();

app.UseStaticFiles();
app.UseExceptionHandler("/errors");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
