using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class ApiController : ControllerBase
{
    protected readonly ISender _mediatorSender;

    public ApiController(ISender mediatorSender)
    {
        _mediatorSender = mediatorSender;
    }
}
