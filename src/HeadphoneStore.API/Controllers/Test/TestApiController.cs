using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.Test;

[ApiController]
[Route("api/v{version:apiVersion}/test")]
[ApiVersion(1)]
public class TestApiController : ControllerBase
{
}
