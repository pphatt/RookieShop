using Asp.Versioning;

using AutoMapper;

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateCategory()
    {
        return Ok();
    }
}
