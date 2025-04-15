using AutoMapper;

using HeadphoneStore.Application.UseCases.V1.Identity.Login;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.Authentication;

[Route("api/v1/[controller]")]
public class AuthenticationController : ApiController
{
    private readonly IMapper _mapper;

    public AuthenticationController(ISender mediatorSender, IMapper mapper) : base(mediatorSender)
    {
        _mapper = mapper;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var mapper = _mapper.Map<LoginCommand>(request);

        var response = await _mediatorSender.Send(mapper);

        return Ok(response);
    }
}
