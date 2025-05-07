using FluentValidation.TestHelper;
using HeadphoneStore.Application.UseCases.V1.Identity.RefreshToken;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.RefreshToken;

[Trait("Authentication", "RefreshToken")]
public class RefreshTokenCommandValidatorTest
{
    private readonly RefreshTokenCommandValidator _validator;

    public RefreshTokenCommandValidatorTest()
    {
        _validator = new RefreshTokenCommandValidator();
    }

    [Fact]
    public async Task ValidCommand_ShouldPassAsync()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            AccessToken = "old_access_token",
            RefreshToken = "old_refresh_token"
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
