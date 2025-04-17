using System.Reflection;

using FluentValidation;

using HeadphoneStore.Contract.Abstracts.Shared;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .GroupBy(
                e => e.PropertyName,
                e => e.ErrorMessage
            )
            .ToDictionary(
                failureGroup => failureGroup.Key,
                failureGroup => failureGroup.ToArray()
            );

        if (failures.Any())
        {
            var errorResponse = new ValidationProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Errors = failures
            };

            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var genericType = typeof(TResponse).GetGenericArguments()[0];

                var failureMethod = typeof(Result)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .FirstOrDefault(m =>
                        m.Name == nameof(Result.Failure) &&
                        m.IsGenericMethodDefinition &&
                        m.GetGenericArguments().Length == 1 &&
                        m.GetParameters().Length == 1 &&
                        m.GetParameters()[0].ParameterType == typeof(object));

                if (failureMethod != null)
                {
                    var genericFailureMethod = failureMethod.MakeGenericMethod(genericType);

                    return (TResponse)genericFailureMethod.Invoke(null, new object[] { errorResponse })!;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Error: Could not find generic Result.Failure<T>(object) in Result class for type {genericType}");
                    return (TResponse)(object)Result.Failure(errorResponse);
                }
            }

            return (TResponse)(object)Result.Failure(errorResponse);
        }

        return await next();
    }
}
