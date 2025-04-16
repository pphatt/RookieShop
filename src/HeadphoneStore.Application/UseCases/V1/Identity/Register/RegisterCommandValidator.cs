using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(256)
            .WithMessage("Email length must be less than 256 characters.")
            .NotNull()
            .NotEmpty()
            .WithMessage("Email must not be empty.");

        RuleFor(x => x.Password)
            .MaximumLength(256)
            .MinimumLength(8)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character (!@#$%^&*()-=) and 8 to 256 characters long.");

        RuleFor(x => x.ConfirmPassword)
            .NotNull()
            .NotEmpty()
            .WithMessage("Confirm Password is required.");

        // Ensuring Password and ConfirmPassword matched
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");
    }
}
