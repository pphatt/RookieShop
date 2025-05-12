using Asp.Versioning;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.UseCases.V1.Identity.GetAllRoles;
using HeadphoneStore.Domain.Constants;
using HeadphoneStore.Shared.Dtos.Identity.Role;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[Authorize]
[ApiVersion(1)]
public class RoleController : BaseApiController
{
    public RoleController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("all")]
    [RequirePermission(Permissions.Function.ROLE, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllRoles()
    {
        var query = new GetAllRolesQuery();

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
