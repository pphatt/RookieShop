using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Application.UseCases.V1.Order.CreateOrder;
using HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;
using HeadphoneStore.Application.UseCases.V1.ProductRating.CreateProductRating;
using HeadphoneStore.Domain.Constants;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Services.Order.CreateOrder;
using HeadphoneStore.Shared.Services.ProductRating.CreateProductRating;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[Authorize]
[ApiVersion(1)]
public class ProductRatingController : BaseApiController
{
    private readonly IMapper _mapper;

    public ProductRatingController(IMediator mediator, IMapper mapper) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpPost("create")]
    [RequirePermission(Permissions.Function.PRODUCT, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateProductRating(CreateProductRatingRequestDto request)
    {
        var mapper = _mapper.Map<CreateProductRatingCommand>(request);

        mapper.CustomerId = User.GetUserId();

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
