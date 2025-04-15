using HeadphoneStore.Application.UseCases.V1.Products.CreateProduct;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.TestController;

[ApiController]
[Route("api/v1/test")]
public class TestController : ApiController
{
    public TestController(ISender mediatorSender) : base(mediatorSender)
    {
    }

    [HttpGet("test-exception")]
    public async Task<IActionResult> TestException()
    {
        var command = new CreateProductCommand();

        var result = await _mediatorSender.Send(command);

        return Ok(result);
    }
}
