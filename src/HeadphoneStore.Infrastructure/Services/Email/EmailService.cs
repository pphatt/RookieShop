using System.Net;
using System.Net.Mail;

using HeadphoneStore.Application.Abstracts.Interface.Services.Mail;
using HeadphoneStore.Infrastructure.DependencyInjection.Options;

using Microsoft.Extensions.Options;

namespace HeadphoneStore.Infrastructure.Services.Mail;

public class EmailService : IEmailService
{
    private readonly EmailOption _mailSettings;

    public EmailService(IOptions<EmailOption> settings)
    {
        _mailSettings = settings.Value;
    }

    public async Task SendEmailAsync(EmailContent email)
    {
        using var client = new SmtpClient(_mailSettings.Host);

        var emailMessage = new MailMessage()
        {
            From = new MailAddress(_mailSettings.Email),
            Subject = email.Subject,
            Body = email.Body,
            IsBodyHtml = true,
        };

        emailMessage.To.Add(new MailAddress(email.ToEmail));

        client.Host = _mailSettings.Host;
        client.Port = _mailSettings.Port;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password);
        client.EnableSsl = true;

        await client.SendMailAsync(emailMessage);
    }
}
