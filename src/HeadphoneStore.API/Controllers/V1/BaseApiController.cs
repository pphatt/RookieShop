using Asp.Versioning;

using HeadphoneStore.Shared.Abstracts.Shared;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HeadphoneStore.API.Controllers.V1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseApiController : ControllerBase
{
    protected readonly IMediator _mediator;

    public BaseApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected IActionResult HandlerFailure(Result result)
    {
        if (result.Errors is ValidationProblemDetails validationProblemDetails)
        {
            return BadRequest(validationProblemDetails);
        }

        if (result.Errors is List<Error> errors)
        {
            return ValidationProblem(errors);
        }

        if (!string.IsNullOrEmpty(result.Message))
        {
            return Problem(title: result.Message, statusCode: StatusCodes.Status400BadRequest);
        }

        return Problem(title: "An error occurred", statusCode: StatusCodes.Status500InternalServerError);
    }

    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        HttpContext.Items["errors"] = errors;

        return Problem(errors[0]);
    }

    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    // This handle return multiple error instead just ones.
    // If we don't have this, probably just return the "Problem(errors[0])" and sometime we want to return multiple errors.
    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelState = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelState.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(modelState);
    }
}
