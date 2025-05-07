using FluentAssertions;
using FluentValidation.TestHelper;
using HeadphoneStore.Application.UseCases.V1.Identity.Login;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.Login;

[Trait("Authentication", "Login")]
public class LoginCommandValidatorTest
{
    private readonly LoginCommandValidator _validator;

    public LoginCommandValidatorTest()
    {
        _validator = new LoginCommandValidator();
    }

    [Fact]
    public async Task ValidCommand_ShouldPassAsync()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "Password123!");

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task InvalidEmail_WithNull_ShouldFailAsync(string email)
    {
        // Arrange
        var command = new LoginCommand(email, "Password123!");

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Email must not be empty.");
    }

    [Theory]
    [InlineData("invalid-email")]
    public async Task InvalidEmail_WithWrongEmailFormat_ShouldFailAsync(string email)
    {
        // Arrange
        var command = new LoginCommand(email, "Password123!");

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("'Email' is not a valid email address.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task InvalidPassword_WithNullOrEmptyPassowrd_ShouldFail(string password)
    {
        // Arrange
        var command = new LoginCommand("test@example.com", password);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character (!@#$%^&*()-=) and 8 to 256 characters long.");
    }

    [Theory]
    [InlineData("short")]
    public async Task InvalidPassword_WithPasswordTooShort_ShouldFail(string password)
    {
        // Arrange
        var command = new LoginCommand("test@example.com", password);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character (!@#$%^&*()-=) and 8 to 256 characters long.");
    }

    [Theory]
    [InlineData("verylongpasswordthatexceeds256characters" +
                "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" +
                "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" +
                "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" +
                "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz123")]
    public async Task InvalidPassword_WithPasswordTooLong_ShouldFail(string password)
    {
        // Arrange
        var command = new LoginCommand("test@example.com", password);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character (!@#$%^&*()-=) and 8 to 256 characters long.");
    }
}
