using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;
using HeadphoneStore.Application.UseCases.V1.Product.DeleteProduct;
using HeadphoneStore.Application.UseCases.V1.Product.GetAllProductsPaged;
using HeadphoneStore.Application.UseCases.V1.Product.GetProductById;
using HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Product;
using HeadphoneStore.Contract.Services.Product.Create;
using HeadphoneStore.Contract.Services.Product.Delete;
using HeadphoneStore.Contract.Services.Product.GetAllPaged;
using HeadphoneStore.Contract.Services.Product.GetById;
using HeadphoneStore.Contract.Services.Product.Update;
using HeadphoneStore.Domain.Constants;

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

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpPut("{Id}")]
    [RequirePermission(Permissions.Function.PRODUCT, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateProductResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductRequestDto request)
    {
        var mapper = _mapper.Map<UpdateProductCommand>(request);

        mapper.Sku = mapper.Name.Slugify();
        mapper.UpdatedBy = User.GetUserId();

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpDelete("{Id}")]
    [RequirePermission(Permissions.Function.PRODUCT, Permissions.Command.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteProductResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> DeleteProduct([FromRoute] DeleteProductRequestDto request)
    {
        var mapper = _mapper.Map<DeleteProductCommand>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
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

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
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

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }
}
