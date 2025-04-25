using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.Application.UseCases.V1.Identity.Login;
using HeadphoneStore.Application.UseCases.V1.Identity.Register;
using HeadphoneStore.Contract.Services.Identity.Login;
using HeadphoneStore.Contract.Services.Identity.Register;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[ApiVersion(1)]
[Authorize]
public class AuthenticationController : BaseApiController
{
    private readonly IMapper _mapper;

    public AuthenticationController(IMediator mediator, IMapper mapper) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var mapper = _mapper.Map<LoginCommand>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var mapper = _mapper.Map<RegisterCommand>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        return Ok();
    }
}
