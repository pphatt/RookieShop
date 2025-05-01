using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Application.UseCases.V1.Category.ActivateCategory;
using HeadphoneStore.Application.UseCases.V1.Category.InactivateCategory;
using HeadphoneStore.Application.UseCases.V1.Product.ActivateProduct;
using HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;
using HeadphoneStore.Application.UseCases.V1.Product.DeleteProduct;
using HeadphoneStore.Application.UseCases.V1.Product.GetAllProductsPaged;
using HeadphoneStore.Application.UseCases.V1.Product.GetProductById;
using HeadphoneStore.Application.UseCases.V1.Product.InactivateProduct;
using HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;
using HeadphoneStore.Domain.Constants;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.Shared.Services.Category.ActivateCategory;
using HeadphoneStore.Shared.Services.Category.InactivateCategory;
using HeadphoneStore.Shared.Services.Product.ActivateProduct;
using HeadphoneStore.Shared.Services.Product.Create;
using HeadphoneStore.Shared.Services.Product.Delete;
using HeadphoneStore.Shared.Services.Product.GetAllPaged;
using HeadphoneStore.Shared.Services.Product.GetById;
using HeadphoneStore.Shared.Services.Product.InactivateProduct;
using HeadphoneStore.Shared.Services.Product.Update;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[Authorize]
[ApiVersion(1)]
public class ProductController : BaseApiController
{
    private readonly IMapper _mapper;

    public ProductController(IMediator mediator, IMapper mapper) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpPost("create")]
    [RequirePermission(Permissions.Function.PRODUCT, Permissions.Command.CREATE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateProductResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequestDto request)
    {
        var mapper = _mapper.Map<CreateProductCommand>(request);

        mapper.Sku = mapper.Name.Slugify();
        mapper.CreatedBy = User.GetUserId();

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{Id}")]
    [RequirePermission(Permissions.Function.PRODUCT, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductRequestDto request)
    {
        var mapper = _mapper.Map<UpdateProductCommand>(request);

        mapper.Sku = mapper.Name.Slugify();
        mapper.UpdatedBy = User.GetUserId();

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{Id}/activate")]
    [RequirePermission(Permissions.Function.PRODUCT, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> ActiveProduct([FromRoute] ActivateProductRequestDto request)
    {
        var mapper = _mapper.Map<ActivateProductCommand>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{Id}/inactivate")]
    [RequirePermission(Permissions.Function.PRODUCT, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> InactiveBrand([FromRoute] InactivateProductRequestDto request)
    {
        var mapper = _mapper.Map<InactivateProductCommand>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{Id}")]
    [RequirePermission(Permissions.Function.PRODUCT, Permissions.Command.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteProductResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> DeleteProduct([FromRoute] DeleteProductRequestDto request)
    {
        var mapper = _mapper.Map<DeleteProductCommand>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("{Id}")]
    [RequirePermission(Permissions.Function.PRODUCT, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductById([FromRoute] GetProductByIdRequestDto request)
    {
        var mapper = _mapper.Map<GetProductByIdQuery>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("pagination")]
    [RequirePermission(Permissions.Function.PRODUCT, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllProductsPagination([FromQuery] GetAllProductPagedRequestDto request)
    {
        var mapper = _mapper.Map<GetAllProductsPagedQuery>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
