using Asp.Versioning;

using HeadphoneStore.Application.UseCases.V1.Identity.Login;
using HeadphoneStore.Application.UseCases.V1.Identity.Logout;
using HeadphoneStore.Application.UseCases.V1.Identity.RefreshToken;
using HeadphoneStore.Application.UseCases.V1.Identity.Register;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Services.Identity.Login;
using HeadphoneStore.Shared.Services.Identity.RefreshToken;
using HeadphoneStore.Shared.Services.Identity.Register;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace HeadphoneStore.API.Controllers.V1;

[ApiVersion(1)]
public class AuthenticationController : BaseApiController
{
    public AuthenticationController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var command = request.MapToCommand();

        var response = await _mediator.Send(command);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var command = request.MapToCommand();

        var response = await _mediator.Send(command);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Logout()
    {
        var accessTokenFromHeader = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

        var command = new LogoutCommand
        {
            AccessToken = accessTokenFromHeader
        };

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenRequestDto request)
    {
        var command = request.MapToCommand();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
