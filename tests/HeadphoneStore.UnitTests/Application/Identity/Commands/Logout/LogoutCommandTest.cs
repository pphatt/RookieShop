using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Identity.Logout;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.Logout;

[Trait("Authentication", "Logout")]
public class LogoutCommandTests : BaseTest
{
    [Fact]
    public void Logout_ShouldMapCorrectly()
    {
        // Arrange
        var accessToken = "some_access_token";

        // Act
        var command = new LogoutCommand
        {
            AccessToken = accessToken
        };

        // Assert
        command.Should().NotBeNull();
        command.AccessToken.Should().Be(accessToken);
    }
}
