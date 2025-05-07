using FluentValidation.TestHelper;

using HeadphoneStore.Application.UseCases.V1.Identity.DeleteUser;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.DeleteUser;

[Trait("Authentication", "DeleteUser")]
public class DeleteUserCommandValidatorTests
{
    private readonly DeleteUserCommandValidator _validator;

    public DeleteUserCommandValidatorTests()
    {
        _validator = new DeleteUserCommandValidator();
    }

    [Fact]
    public async Task Validate_ShouldPassAsync_AnyCommand()
    {
        // Arrange
        var command = new DeleteUserCommand
        {
            Id = Guid.NewGuid()
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
