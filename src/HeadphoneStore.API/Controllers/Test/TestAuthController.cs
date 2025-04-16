using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.Test;

[Tags("Test")]
[ApiVersion(1)]
public class TestAuthController : TestApiController
{
    [HttpGet("auth")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion(1)]
    public IActionResult TestAuth()
    {
        return Ok("Access auth route successfully.");
    }
}
