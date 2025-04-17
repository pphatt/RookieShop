using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;
using HeadphoneStore.Contract.Services.Category.Create;
using HeadphoneStore.Contract.Services.Identity.Login;

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

        mapper.CreatedBy = Guid.Parse("695C0301-D309-4E4E-9B19-7DA747888ED1");

        var response = await _mediator.Send(mapper);

        if (response.IsFailure is true)
            return HandlerFailure(response);

        return Ok(response);
    }
}
