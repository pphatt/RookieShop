using Asp.Versioning;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Application.UseCases.V1.Order.CreateOrder;
using HeadphoneStore.Application.UseCases.V1.Order.GetAllOrdersPagination;
using HeadphoneStore.Application.UseCases.V1.Order.GetOrderById;
using HeadphoneStore.Domain.Constants;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Order;
using HeadphoneStore.Shared.Services.Order.CreateOrder;
using HeadphoneStore.Shared.Services.Order.GetAllOrdersPagination;
using HeadphoneStore.Shared.Services.Order.GetOrderById;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[Authorize]
[ApiVersion(1)]
public class OrderController : BaseApiController
{
    public OrderController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("create")]
    [RequirePermission(Permissions.Function.ORDER, Permissions.Command.CREATE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateOrder(CreateOrderRequestDto request)
    {
        var command = request.MapToCommand(User.GetUserId());

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("{Id}")]
    [RequirePermission(Permissions.Function.ORDER, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<OrderDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetOrderById([FromRoute] GetOrderByIdRequestDto request)
    {
        var query = request.MapToQuery(User.GetUserId());

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("pagination")]
    [RequirePermission(Permissions.Function.ORDER, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<List<OrderDto>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAllOrdersPagination([FromQuery] GetAllOrdersPaginationRequestDto request)
    {
        var query = request.MapToQuery(User.GetUserId());

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
