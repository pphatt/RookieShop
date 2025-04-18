using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1.Test;

[ApiController]
[Route("api/v{version:apiVersion}/test")]
[ApiVersion(1)]
public class TestApiController : ControllerBase
{
}
