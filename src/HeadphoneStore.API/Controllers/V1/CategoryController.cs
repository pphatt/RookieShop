using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;
using HeadphoneStore.Application.UseCases.V1.Category.DeleteCategory;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllCategories;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesWithSubCategories;
using HeadphoneStore.Application.UseCases.V1.Category.GetAllSubCategories;
using HeadphoneStore.Application.UseCases.V1.Category.GetCategoryById;
using HeadphoneStore.Application.UseCases.V1.Category.UpdateCategory;
using HeadphoneStore.Contract.Dtos.Category;
using HeadphoneStore.Contract.Services.Category.Create;
using HeadphoneStore.Contract.Services.Category.Delete;
using HeadphoneStore.Contract.Services.Category.GetCategoryById;
using HeadphoneStore.Contract.Services.Category.Update;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[ApiVersion(1)]
[Authorize]
public class CategoryController : BaseApiController
{
    private readonly IMapper _mapper;

    public CategoryController(IMediator mediator, IMapper mapper) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateCategoryResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryRequestDto request)
    {
        var mapper = _mapper.Map<CreateCategoryCommand>(request);

        mapper.CreatedBy = Guid.Parse("F50F0A80-12DC-4BD2-8861-8D0A8B3A6E96");

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpPut("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateCategoryResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryRequestDto request)
    {
        var mapper = _mapper.Map<UpdateCategoryCommand>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }

    [HttpDelete("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteCategoryResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteCategory([FromRoute] DeleteCategoryRequestDto request)
    {
        var mapper = _mapper.Map<DeleteCategoryCommand>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok();
    }

    [HttpGet("{Id}")]
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

    [HttpGet("all")]
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
