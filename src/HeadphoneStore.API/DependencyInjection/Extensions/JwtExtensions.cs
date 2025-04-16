using System.Net;
using System.Net.Mime;
using System.Text;

using HeadphoneStore.Infrastructure.DependencyInjection.Options;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

namespace HeadphoneStore.API.DependencyInjection.Extensions;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        JwtOption jwtOptions = new JwtOption();

        configuration.Bind(nameof(JwtOption), jwtOptions);

        services.AddSingleton(Options.Create(jwtOptions));

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.IncludeErrorDetails = true;

            /**
             * Storing the JWT in the AuthenticationProperties allows you to retrieve it from elsewhere within your application.
             * public async Task<IActionResult> SomeAction()
                {
                    // using Microsoft.AspNetCore.Authentication;
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    // ...
                }
             */
            options.SaveToken = true; // Save token into AuthenticationProperties

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, // on production make it true
                ValidateAudience = true, // on production make it true
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                },

                OnAuthenticationFailed = context =>
                {
                    var result = string.Empty;

                    if (context.Response.HasStarted)
                    {
                        return Task.CompletedTask;
                    }

                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    // is it token expired.
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                        result = JsonConvert.SerializeObject(new
                        {
                            message = "Token is expired.",
                            statusCode = (int)HttpStatusCode.Unauthorized,
                            status = "Unauthorized"
                        });
                    }
                    // or internal error (or this can be happened when access token is incorrect).
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        result = JsonConvert.SerializeObject(new { message = "Internal server error." });
                    }

                    return context.Response.WriteAsync(result);
                },

                OnChallenge = context =>
                {
                    context.HandleResponse();

                    // Have to check this because when authorized access api failed,
                    // asp.net core web api will redirect to the 401 page and also we send the 401 message too.
                    // This will make the asp.net throw error.
                    if (context.Response.HasStarted)
                    {
                        return Task.CompletedTask;
                    }

                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var result = JsonConvert.SerializeObject(new { message = "You are not authorized." });

                    return context.Response.WriteAsync(result);
                },

                OnForbidden = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var result = JsonConvert.SerializeObject(new { message = "You are not authorized to access these resources." });

                    return context.Response.WriteAsync(result);
                }
            };
        });

        services.AddAuthorization();

        return services;
    }
}
