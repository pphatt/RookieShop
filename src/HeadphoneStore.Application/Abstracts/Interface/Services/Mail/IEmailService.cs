using AutoMapper.Internal;

namespace HeadphoneStore.Application.Abstracts.Interface.Services.Mail;

public interface IEmailService
{
    Task SendEmailAsync(EmailContent email);
}
