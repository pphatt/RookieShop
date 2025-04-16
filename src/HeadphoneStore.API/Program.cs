using System.Text.Json;
using System.Text.Json.Serialization;

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

// API Layer
builder.Services.AddJwtAuthentication(builder.Configuration); // authentication and authorization

// Application Layer
builder.Services.AddMediatRApplication();
builder.Services.AddAutoMapperApplication();

// Infrastructure Layer
builder.Services.AddInfrastructureDependenciesLayer(builder.Configuration);
builder.Services.ConfigureMailOptionsInfrastructure(builder.Configuration.GetSection(nameof(EmailOption)));
builder.Services.AddHttpContextAccessor();

// Persistence Layer
builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration);
builder.Services.AddSqlServerPersistence(builder.Configuration);
builder.Services.AddDbIdentity();

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

// Application Layer
app.AddAutoMapperValidationApplication();

app.Run();
