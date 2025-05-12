using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Identity.Login;
using HeadphoneStore.Shared.Services.Identity.Login;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.Login;

[Trait("Authentication", "Login")]
public class LoginCommandTest : BaseTest
{
    [Fact]
    public void Login_ShouldMapCorrectly()
    {
        // Arrange
        var requestDto = new LoginRequestDto
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        // Act
        var command = requestDto.MapToCommand();

        // Assert
        command.Should().NotBeNull();
        command.Email.Should().Be(requestDto.Email);
        command.Password.Should().Be(requestDto.Password);
    }
}
