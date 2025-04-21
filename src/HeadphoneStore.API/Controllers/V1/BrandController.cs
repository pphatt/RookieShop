using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;
using HeadphoneStore.Contract.Services.Brand.Create;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateBrandResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateBrand([FromForm] CreateBrandRequestDto request)
    {
        var mapper = _mapper.Map<CreateBrandCommand>(request);

        mapper.CreatedBy = Guid.Parse("5E640E2D-F4AA-4776-85BC-F860A3E58F31");

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }
}
