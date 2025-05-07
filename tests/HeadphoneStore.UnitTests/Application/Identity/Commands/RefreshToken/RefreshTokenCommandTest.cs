using FluentAssertions;
using HeadphoneStore.Application.UseCases.V1.Identity.RefreshToken;
using HeadphoneStore.Shared.Services.Identity.RefreshToken;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.RefreshToken;

[Trait("Authentication", "RefreshToken")]
public class RefreshTokenCommandTest : BaseTest
{
    [Fact]
    public void RefreshToken_ShouldMapCorrectly()
    {
        // Arrange
        var requestDto = new RefreshTokenRequestDto
        {
            AccessToken = "old-access-token",
            RefreshToken = "old-refresh-token"
        };

        // Act
        var command = _mapper.Map<RefreshTokenCommand>(requestDto);

        // Assert
        command.Should().NotBeNull();
        command.AccessToken.Should().Be(requestDto.AccessToken);
        command.RefreshToken.Should().Be(requestDto.RefreshToken);
    }
}
