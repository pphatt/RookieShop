using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

using HeadphoneStore.Shared.Abstracts.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HeadphoneStore.API.Middlewares;

internal sealed class GlobalExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly ApiBehaviorOptions _options;

    public GlobalExceptionHandlerMiddleware(
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IOptions<ApiBehaviorOptions> options)
    {
        _logger = logger;
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails();

        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;

        if (exception.InnerException != null)
        {
            var code = GetExceptionCodeFromInnerException(exception.InnerException);
            string description = GetExceptionDescriptionFromInnerException(exception.InnerException);

            var statusCode = GetExceptionTypeFromInnerException(exception.InnerException) switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            problemDetails.Status = statusCode;

            if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }

            if (description is not null)
            {
                problemDetails.Title = description;
            }

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                problemDetails.Detail = description;
                problemDetails.Extensions.Add("error", "Internal Server Error");
            }
            else
            {
                problemDetails.Extensions.Add("errorCodes", new List<string>() { code });
            }
        }
        else
        {
            problemDetails.Status = StatusCodes.Status500InternalServerError;

            if (_options.ClientErrorMapping.TryGetValue((int)problemDetails.Status, out var clientErrorData))
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }
        }

        context.Response.StatusCode = (int)problemDetails.Status;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

        await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails, options);
    }

    private ErrorType GetExceptionTypeFromInnerException(Exception innerException)
    {
        var typeProperty = innerException.GetType().GetProperty("Type");

        if (typeProperty != null)
        {
            var typeValue = typeProperty.GetValue(innerException)?.ToString();

            if (!string.IsNullOrEmpty(typeValue))
            {
                Enum.TryParse<ErrorType>(typeValue.ToUpperInvariant(), true, out var errorType);

                return errorType;
            }
        }

        return ErrorType.Failure;
    }

    private string GetExceptionDescriptionFromInnerException(Exception innerException)
    {
        var descriptionProperty = innerException.GetType().GetProperty("Description");

        if (descriptionProperty != null)
        {
            return descriptionProperty.GetValue(innerException)?.ToString() ?? innerException.Message;
        }

        return innerException.Message;
    }

    private string GetExceptionCodeFromInnerException(Exception innerException)
    {
        var codeProperty = innerException.GetType().GetProperty("Code");

        if (codeProperty != null && codeProperty.GetValue(innerException) is string code)
        {
            return code;
        }

        return "Something went wrong. Internal server error.";
    }
}
