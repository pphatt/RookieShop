using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.Test;

[Tags("Test")]
public class TestAuthController : TestApiController
{
    [HttpGet("auth")]
    [Authorize]
    public IActionResult TestAuth()
    {
        return Ok("Access auth route successfully.");
    }
}
