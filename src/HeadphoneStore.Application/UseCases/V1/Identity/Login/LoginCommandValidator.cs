using System.Text.RegularExpressions;

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
            .NotNull()
            .NotEmpty()
            .Must(IsValidPassword)
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character (!@#$%^&*()-=) and 8 to 256 characters long.");
    }

    private bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return false;

        if (password.Length < 8 || password.Length > 256)
            return false;

        var hasUpperCase = Regex.IsMatch(password, "[A-Z]");
        var hasLowerCase = Regex.IsMatch(password, "[a-z]");
        var hasDigit = Regex.IsMatch(password, "[0-9]");
        var hasSpecial = Regex.IsMatch(password, @"[!@#$%^&*()\-=]");

        return hasUpperCase && hasLowerCase && hasDigit && hasSpecial;
    }
}
