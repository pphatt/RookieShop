using Asp.Versioning;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Application.UseCases.V1.Identity.CreateUser;
using HeadphoneStore.Application.UseCases.V1.Identity.DeleteUser;
using HeadphoneStore.Application.UseCases.V1.Identity.GetAllUsersPaged;
using HeadphoneStore.Application.UseCases.V1.Identity.GetUserById;
using HeadphoneStore.Application.UseCases.V1.Identity.UpdateUser;
using HeadphoneStore.Application.UseCases.V1.Identity.WhoAmI;
using HeadphoneStore.Domain.Constants;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Identity.User;
using HeadphoneStore.Shared.Services.Identity.CreateUser;
using HeadphoneStore.Shared.Services.Identity.DeleteUser;
using HeadphoneStore.Shared.Services.Identity.GetAllUserPaged;
using HeadphoneStore.Shared.Services.Identity.GetUserById;
using HeadphoneStore.Shared.Services.Identity.UpdateUser;
using HeadphoneStore.Shared.Services.Product.Create;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[Authorize]
[ApiVersion(1)]
public class UserController : BaseApiController
{
    public UserController(IMediator mediator) : base(mediator)
    {
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

    [HttpPost("create")]
    [RequirePermission(Permissions.Function.USER, Permissions.Command.CREATE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateUser([FromForm] CreateUserRequestDto request)
    {
        var command = request.MapToCommand();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{Id}")]
    [RequirePermission(Permissions.Function.USER, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateUser(UpdateUserRequestDto request)
    {
        var command = request.MapToCommand();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{Id}")]
    [RequirePermission(Permissions.Function.USER, Permissions.Command.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> DeleteUser(DeleteUserRequestDto request)
    {
        var command = request.MapToCommand();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("{Id}")]
    [RequirePermission(Permissions.Function.USER, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetUserById([FromRoute] GetUserByIdRequestDto request)
    {
        var query = request.MapToQuery();

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("pagination")]
    [RequirePermission(Permissions.Function.USER, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<UserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAllUserPagination([FromQuery] GetAllUsersPagedRequestDto request)
    {
        var query = request.MapToQuery();

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
