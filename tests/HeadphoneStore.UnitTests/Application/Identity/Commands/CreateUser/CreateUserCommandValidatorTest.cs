using FluentValidation.TestHelper;

using HeadphoneStore.Application.UseCases.V1.Identity.CreateUser;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.CreateUser;

[Trait("Authentication", "CreateUser")]
public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        _validator = new CreateUserCommandValidator();
    }

    [Fact]
    public async Task ValidCommand_ShouldPassAsync()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
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
