using HeadphoneStore.API.Controllers.Test;
using HeadphoneStore.Application.Abstracts.Interface.Services.Mail;

using Microsoft.AspNetCore.Mvc;

namespace Server.Api.Controllers.TestApi;

[Tags("Test")]
public class TestEmailController : TestApiController
{
    private readonly IEmailService _mailService;

    public TestEmailController(IEmailService mailService)
    {
        _mailService = mailService;
    }

    [HttpGet("email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TestEmail()
    {
        var emailRequest = new EmailContent
        {
            ToEmail = "phatvu080903@gmail.com",
            Subject = "Test",
            Body = "Test",
        };

        await _mailService.SendEmailAsync(emailRequest);

        return Ok("Email send successfully.");
    }
}
