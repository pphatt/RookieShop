using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.Application.UseCases.V1.Identity.Login;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.Authentication;

[ApiVersion(1)]
public class AuthenticationController : ApiController
{
    private readonly IMapper _mapper;

    public AuthenticationController(ISender mediatorSender, IMapper mapper) : base(mediatorSender)
    {
        _mapper = mapper;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var mapper = _mapper.Map<LoginCommand>(request);

        var response = await _mediatorSender.Send(mapper);

        return Ok(response);
    }
}
