using FluentValidation.TestHelper;

using HeadphoneStore.Application.UseCases.V1.Identity.Logout;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.Logout;

[Trait("Authentication", "Logout")]
public class LogoutCommandValidatorTests
{
    private readonly LogoutCommandValidator _validator;

    public LogoutCommandValidatorTests()
    {
        _validator = new LogoutCommandValidator();
    }

    [Fact]
    public async Task ValidCommand_ShouldPassAsync()
    {
        // Arrange
        var command = new LogoutCommand
        {
            AccessToken = "some_access_token"
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
