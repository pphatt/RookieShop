using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.Application.UseCases.V1.Identity.Login;
using HeadphoneStore.Application.UseCases.V1.Identity.Register;
using HeadphoneStore.Contract.Services.Identity.Login;
using HeadphoneStore.Contract.Services.Identity.Register;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[ApiVersion(1)]
public class AuthenticationController : ApiController
{
    private readonly IMapper _mapper;

    public AuthenticationController(ISender mediatorSender, IMapper mapper) : base(mediatorSender)
    {
        _mapper = mapper;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var mapper = _mapper.Map<LoginCommand>(request);

        var response = await _mediatorSender.Send(mapper);

        if (response.IsFailure is true)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var mapper = _mapper.Map<RegisterCommand>(request);

        var response = await _mediatorSender.Send(mapper);

        if (response.IsFailure is true)
            return HandlerFailure(response);

        return Ok(response);
    }
}
