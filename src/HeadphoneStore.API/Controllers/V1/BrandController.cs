using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Application.UseCases.V1.Brand.ActiveBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.BulkDeleteBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrands;
using HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsByProductProperties;
using HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsPaged;
using HeadphoneStore.Application.UseCases.V1.Brand.GetBrandById;
using HeadphoneStore.Application.UseCases.V1.Brand.InactiveBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;
using HeadphoneStore.Domain.Constants;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;
using HeadphoneStore.Shared.Services.Brand.ActiveBrand;
using HeadphoneStore.Shared.Services.Brand.BulkDelete;
using HeadphoneStore.Shared.Services.Brand.Create;
using HeadphoneStore.Shared.Services.Brand.Delete;
using HeadphoneStore.Shared.Services.Brand.GetAllBrandsByProductProperties;
using HeadphoneStore.Shared.Services.Brand.GetAllPaged;
using HeadphoneStore.Shared.Services.Brand.GetById;
using HeadphoneStore.Shared.Services.Brand.InactiveBrand;
using HeadphoneStore.Shared.Services.Brand.Update;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[Authorize]
[ApiVersion(1)]
public class BrandController : BaseApiController
{
    private readonly IMapper _mapper;

    public BrandController(IMediator mediator, IMapper mapper) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpPost("create")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.CREATE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateBrandResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateBrand([FromForm] CreateBrandRequestDto request)
    {
        var mapper = _mapper.Map<CreateBrandCommand>(request);

        mapper.Slug = request.Slug ?? mapper.Name.Slugify();
        mapper.CreatedBy = User.GetUserId();

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{Id}")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateBrand([FromForm] UpdateBrandRequestDto request)
    {
        var mapper = _mapper.Map<UpdateBrandCommand>(request);

        mapper.UpdatedBy = User.GetUserId();

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{Id}/activate")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> ActiveBrand([FromRoute] ActivateBrandRequestDto request)
    {
        var mapper = _mapper.Map<ActivateBrandCommand>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{Id}/inactivate")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> InactiveBrand([FromRoute] InactivateBrandRequestDto request)
    {
        var mapper = _mapper.Map<InactivateBrandCommand>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{Id}")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> DeleteBrand([FromRoute] DeleteBrandRequestDto request)
    {
        var mapper = _mapper.Map<DeleteBrandCommand>(request);

        mapper.UpdatedBy = User.GetUserId();

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("bulk-delete")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> BulkDeleteBrand([FromForm] BulkDeleteBrandRequestDto request)
    {
        var mapper = _mapper.Map<BulkDeleteBrandCommand>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("{Id}")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetBrandById([FromRoute] GetBrandByIdRequestDto request)
    {
        var mapper = _mapper.Map<GetBrandByIdQuery>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("all")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBrands()
    {
        var query = new GetAllBrandsQuery();

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("pagination")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<BrandDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBrandsPagination([FromQuery] GetAllBrandsPagedRequestDto request)
    {
        var mapper = _mapper.Map<GetAllBrandsPagedQuery>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("all-brands-filtered-by-product-properties")]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBrandsFilteredByProductProperties([FromQuery] GetAllBrandsByProductPropertiesRequestDto request)
    {
        var mapper = _mapper.Map<GetAllBrandsByProductPropertiesQuery>(request);

        var result = await _mediator.Send(mapper);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
