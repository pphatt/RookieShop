using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.Test;

[Tags("Test")]
public class TestAuthController : TestApiController
{
    [HttpGet("auth")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult TestAuth()
    {
        return Ok("Access auth route successfully.");
    }
}
