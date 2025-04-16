namespace HeadphoneStore.Application.Abstracts.Interface.Services.Mail;

public class EmailContent
{
    public string ToEmail { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
}
