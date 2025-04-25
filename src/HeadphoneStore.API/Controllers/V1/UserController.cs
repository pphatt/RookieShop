using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Application.UseCases.V1.Identity.WhoAmI;
using HeadphoneStore.Contract.Services.Product.Create;
using HeadphoneStore.Domain.Constants;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[Authorize]
[ApiVersion(1)]
public class UserController : BaseApiController
{
    private readonly IMapper _mapper;

    public UserController(IMapper mapper, IMediator mediator) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpGet("whoami")]
    [RequirePermission(Permissions.Function.USER, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateProductResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> WhoAmI()
    {
        var whoAmIQuery = new WhoAmIQuery
        {
            UserId = User.GetUserId().ToString()
        };

        var response = await _mediator.Send(whoAmIQuery);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }
}
