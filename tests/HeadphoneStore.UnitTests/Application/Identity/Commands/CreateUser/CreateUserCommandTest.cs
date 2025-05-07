using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Identity.CreateUser;
using HeadphoneStore.Application.UseCases.V1.Identity.Login;
using HeadphoneStore.Shared.Services.Identity.CreateUser;

using Microsoft.VisualStudio.TestPlatform.Common;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.CreateUser;

[Trait("Authentication", "CreateUser")]
public class CreateUserCommandTests : BaseTest
{
    [Fact]
    public void CreateUser_ShouldMapCorrectly()
    {
        // Arrange
        var requestDto = new CreateUserRequestDto
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };

        // Act
        var command = _mapper.Map<CreateUserCommand>(requestDto);

        // Assert
        command.Should().NotBeNull();
        command.Email.Should().Be("test@example.com");
        command.FirstName.Should().Be("John");
        command.LastName.Should().Be("Doe");
        command.PhoneNumber.Should().Be("1234567890");
        command.RoleId.Should().NotBeEmpty();
        command.Status.Should().Be("Active");
    }
}
