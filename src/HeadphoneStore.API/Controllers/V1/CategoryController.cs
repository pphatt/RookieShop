using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;
using HeadphoneStore.Application.UseCases.V1.Category.DeleteCategory;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllCategories;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesPaged;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesWithSubCategories;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllSubCategories;
using HeadphoneStore.Application.UseCases.V1.Category.GetCategoryById;
using HeadphoneStore.Application.UseCases.V1.Category.UpdateCategory;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Category;
using HeadphoneStore.Contract.Services.Category.Create;
using HeadphoneStore.Contract.Services.Category.Delete;
using HeadphoneStore.Contract.Services.Category.GetAllPaged;
using HeadphoneStore.Contract.Services.Category.GetCategoryById;
using HeadphoneStore.Contract.Services.Category.Update;
using HeadphoneStore.Domain.Constants;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[Authorize]
[ApiVersion(1)]
public class CategoryController : BaseApiController
{
    private readonly IMapper _mapper;

    public CategoryController(IMediator mediator, IMapper mapper) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpPost("create")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.CREATE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateCategoryResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryRequestDto request)
    {
        var mapper = _mapper.Map<CreateCategoryCommand>(request);

        mapper.CreatedBy = User.GetUserId();

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpPut("{Id}")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateCategoryResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryRequestDto request)
    {
        var mapper = _mapper.Map<UpdateCategoryCommand>(request);

        mapper.UpdatedBy = User.GetUserId();

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpDelete("{Id}")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteCategoryResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> DeleteCategory([FromRoute] DeleteCategoryRequestDto request)
    {
        var mapper = _mapper.Map<DeleteCategoryCommand>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok();
    }

    [HttpGet("{Id}")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateCategoryResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategoryById([FromRoute] GetCategoryByIdRequestDto request)
    {
        var mapper = _mapper.Map<GetCategoryByIdQuery>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpGet("pagination")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCategoriesPagination([FromQuery] GetAllCategoriesPagedRequestDto request)
    {
        var mapper = _mapper.Map<GetAllCategoriesPagedQuery>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpGet("all")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCategories()
    {
        var query = new GetAllCategoriesQuery();

        var response = await _mediator.Send(query);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpGet("all-sub")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllSubCategories()
    {
        var query = new GetAllSubCategoriesQuery();

        var response = await _mediator.Send(query);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpGet("all-with-sub")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCategoriesWithSubCategories()
    {
        var query = new GetAllCategoriesWithSubCategoriesQuery();

        var response = await _mediator.Send(query);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }
}
