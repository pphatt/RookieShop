using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
    }
}
