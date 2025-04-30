using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Application.UseCases.V1.Identity.CreateUser;
using HeadphoneStore.Application.UseCases.V1.Identity.DeleteUser;
using HeadphoneStore.Application.UseCases.V1.Identity.GetAllUserPaged;
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

    [HttpPost("create")]
    [RequirePermission(Permissions.Function.USER, Permissions.Command.CREATE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateUser([FromForm] CreateUserRequestDto request)
    {
        var mapper = _mapper.Map<CreateUserCommand>(request);

        var result = await _mediator.Send(mapper);

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
        var mapper = _mapper.Map<UpdateUserCommand>(request);

        var result = await _mediator.Send(mapper);

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
        var mapper = _mapper.Map<DeleteUserCommand>(request);

        var result = await _mediator.Send(mapper);

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
        var mapper = _mapper.Map<GetUserByIdQuery>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("pagination")]
    [RequirePermission(Permissions.Function.USER, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<UserDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAllUserPagination([FromQuery] GetAllUserPagedRequestDto request)
    {
        var mapper = _mapper.Map<GetAllUserPagedQuery>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
