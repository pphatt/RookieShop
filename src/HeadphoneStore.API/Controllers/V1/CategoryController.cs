using Asp.Versioning;

using HeadphoneStore.API.Authorization;
using HeadphoneStore.Application.UseCases.V1.Category.ActivateCategory;
using HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;
using HeadphoneStore.Application.UseCases.V1.Category.DeleteCategory;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllCategories;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesPaged;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesWithSubCategories;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllFirstLevelCategories;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllSubCategories;
using HeadphoneStore.Application.UseCases.V1.Category.GetCategoryById;
using HeadphoneStore.Application.UseCases.V1.Category.InactivateCategory;
using HeadphoneStore.Application.UseCases.V1.Category.UpdateCategory;
using HeadphoneStore.Domain.Constants;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;
using HeadphoneStore.Shared.Services.Category.ActivateCategory;
using HeadphoneStore.Shared.Services.Category.Create;
using HeadphoneStore.Shared.Services.Category.Delete;
using HeadphoneStore.Shared.Services.Category.GetAllCategories;
using HeadphoneStore.Shared.Services.Category.GetAllPaged;
using HeadphoneStore.Shared.Services.Category.GetCategoryById;
using HeadphoneStore.Shared.Services.Category.InactivateCategory;
using HeadphoneStore.Shared.Services.Category.Update;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[Authorize]
[ApiVersion(1)]
public class CategoryController : BaseApiController
{
    public CategoryController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("create")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.CREATE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateCategoryResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryRequestDto request)
    {
        var command = request.MapToCommand();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{Id}")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateCategoryResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryRequestDto request)
    {
        var command = request.MapToCommand();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{Id}/activate")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> ActiveCategory([FromRoute] ActivateCategoryRequestDto request)
    {
        var command = request.MapToCommand();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPut("{Id}/inactivate")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.EDIT)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> InactiveBrand([FromRoute] InactivateCategoryRequestDto request)
    {
        var command = request.MapToCommand();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpDelete("{Id}")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.DELETE)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteCategoryResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> DeleteCategory([FromRoute] DeleteCategoryRequestDto request)
    {
        var command = request.MapToCommand();

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("{Id}")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateCategoryResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategoryById([FromRoute] GetCategoryByIdRequestDto request)
    {
        var query = request.MapToQuery();

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("pagination")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCategoriesPagination([FromQuery] GetAllCategoriesPagedRequestDto request)
    {
        var query = request.MapToQuery();

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("all")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCategories([FromQuery] GetAllCategoriesRequestDto request)
    {
        var query = request.MapToQuery();

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("all-first-level")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllFirstLevelCategories()
    {
        var query = new GetAllFirstLevelCategoriesQuery();

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
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

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("all-with-sub")]
    [RequirePermission(Permissions.Function.CATEGORY, Permissions.Command.VIEW)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCategoriesWithSubCategories([FromQuery] string CategorySlug)
    {
        var query = new GetAllCategoriesWithSubCategoriesQuery(CategorySlug);

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
