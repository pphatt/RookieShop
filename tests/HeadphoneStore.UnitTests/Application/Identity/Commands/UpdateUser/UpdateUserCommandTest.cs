using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Identity.UpdateUser;
using HeadphoneStore.Shared.Services.Identity.UpdateUser;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.UpdateUser;


[Trait("Authentication", "UpdateUser")]
public class UpdateUserCommandTests : BaseTest
{
    [Fact]
    public void UpdateUserCommand_ShouldInitializeCorrectly()
    {
        // Arrange
        var requestDto = new UpdateUserRequestDto
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };

        // Act
        var command = _mapper.Map<UpdateUserCommand>(requestDto);

        // Assert
        command.Should().NotBeNull();
        command.Id.Should().NotBeEmpty();
        command.FirstName.Should().Be("John");
        command.LastName.Should().Be("Doe");
        command.PhoneNumber.Should().Be("1234567890");
        command.RoleId.Should().NotBeEmpty();
        command.Status.Should().Be("Active");
    }
}
