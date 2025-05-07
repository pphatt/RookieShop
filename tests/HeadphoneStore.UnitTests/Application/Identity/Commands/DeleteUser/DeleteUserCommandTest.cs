using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Identity.DeleteUser;
using HeadphoneStore.Shared.Services.Identity.DeleteUser;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.DeleteUser;

[Trait("Authentication", "DeleteUser")]
public class DeleteUserCommandTests : BaseTest
{
    [Fact]
    public void DeleteUser_ShouldMapCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var requestDto = new DeleteUserRequestDto
        {
            Id = userId.ToString()
        };

        // Act
        var command = _mapper.Map<DeleteUserCommand>(requestDto);

        // Assert
        command.Should().NotBeNull();
        command.Id.Should().Be(userId);
    }
}
