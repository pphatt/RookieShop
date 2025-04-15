using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(dto => dto.Email)
            .EmailAddress()
            .MaximumLength(256)
            .WithMessage("Email length must be less than 256 characters.")
            .NotNull()
            .NotEmpty()
            .WithMessage("Email must not be empty.");

        RuleFor(dto => dto.Password)
            .MaximumLength(256)
            .MinimumLength(8)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character (!@#$%^&*()-=) and 8 to 256 characters long.");
    }
}
