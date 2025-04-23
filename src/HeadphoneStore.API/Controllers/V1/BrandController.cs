using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Application.UseCases.V1.Brand.BulkDeleteBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;
using HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrands;
using HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsPaged;
using HeadphoneStore.Application.UseCases.V1.Brand.GetBrandById;
using HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Brand;
using HeadphoneStore.Contract.Services.Brand.BulkDelete;
using HeadphoneStore.Contract.Services.Brand.Create;
using HeadphoneStore.Contract.Services.Brand.Delete;
using HeadphoneStore.Contract.Services.Brand.GetAll;
using HeadphoneStore.Contract.Services.Brand.GetAllPaged;
using HeadphoneStore.Contract.Services.Brand.GetById;
using HeadphoneStore.Contract.Services.Brand.Update;
using HeadphoneStore.Domain.Constants;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

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

        mapper.CreatedBy = User.GetUserId();

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpPut("{Id}")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateBrandRequestDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateBrand([FromForm] UpdateBrandRequestDto request)
    {
        var mapper = _mapper.Map<UpdateBrandCommand>(request);

        mapper.UpdatedBy = User.GetUserId();

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpDelete("{Id}")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteBrandResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> DeleteBrand([FromRoute] DeleteBrandRequestDto request)
    {
        var mapper = _mapper.Map<DeleteBrandCommand>(request);

        mapper.UpdatedBy = User.GetUserId();

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpDelete("bulk-delete")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BulkDeleteBrandResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> BulkDeleteBrand([FromForm] BulkDeleteBrandRequestDto request)
    {
        var mapper = _mapper.Map<BulkDeleteBrandCommand>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpGet("{Id}")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetBrandById([FromRoute] GetBrandByIdRequestDto request)
    {
        var mapper = _mapper.Map<GetBrandByIdQuery>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpGet("all")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAllBrands()
    {
        var query = new GetAllBrandsQuery();

        var response = await _mediator.Send(query);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpGet("pagination")]
    [RequirePermission(Permissions.Function.BRAND, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<BrandDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAllBrandsPagination([FromQuery] GetAllBrandsPagedRequestDto request)
    {
        var mapper = _mapper.Map<GetAllBrandsPagedQuery>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }
}
