using FluentValidation.TestHelper;

using HeadphoneStore.Application.UseCases.V1.Identity.UpdateUser;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.UpdateUser;

[Trait("Authentication", "UpdateUser")]
public class UpdateUserCommandValidatorTests
{
    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserCommandValidatorTests()
    {
        _validator = new UpdateUserCommandValidator();
    }

    [Fact]
    public async Task Validate_ShouldPassAsync_AnyCommand()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
