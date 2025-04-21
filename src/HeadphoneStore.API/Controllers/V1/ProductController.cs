using Asp.Versioning;

using AutoMapper;

using HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;
using HeadphoneStore.Contract.Services.Product.Create;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1;

[ApiVersion(1)]
public class ProductController : BaseApiController
{
    private readonly IMapper _mapper;

    public ProductController(IMediator mediator, IMapper mapper) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateProductResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequestDto request)
    {
        var mapper = _mapper.Map<CreateProductCommand>(request);

        var response = await _mediator.Send(mapper);

        if (response.IsFailure)
            return HandlerFailure(response);

        return Ok(response);
    }
}
