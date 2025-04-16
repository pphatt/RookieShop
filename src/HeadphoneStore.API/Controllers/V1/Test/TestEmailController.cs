using Asp.Versioning;

using HeadphoneStore.Application.Abstracts.Interface.Services.Mail;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1.Test;

[Tags("Test")]
[ApiVersion(1)]
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
    [MapToApiVersion(1)]
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
