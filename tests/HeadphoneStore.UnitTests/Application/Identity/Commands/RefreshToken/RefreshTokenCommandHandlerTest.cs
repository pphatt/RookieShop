using FluentAssertions;
using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.UseCases.V1.Identity.RefreshToken;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Services.Identity.Login;
using HeadphoneStore.Shared.Services.Identity.RefreshToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.RefreshToken;

using Exceptions = Domain.Exceptions.Exceptions;

[Trait("Authentication", "RefreshToken")]
public class RefreshTokenCommandHandlerTest : BaseTest
{
    private readonly RefreshTokenCommandHandler _commandHandler;

    private readonly Mock<ILogger<RefreshTokenCommandHandler>> _loggerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IClaimsTransformation> _claimsTransformationMock;

    public RefreshTokenCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<RefreshTokenCommandHandler>>();
        _tokenServiceMock = new Mock<ITokenService>();
        _cacheServiceMock = new Mock<ICacheService>();
        _claimsTransformationMock = new Mock<IClaimsTransformation>();

        _commandHandler = new RefreshTokenCommandHandler(
            _tokenServiceMock.Object,
            _cacheServiceMock.Object,
            _claimsTransformationMock.Object,
            _mockUserManager.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnSuccessResult_ValidTokens()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            AccessToken = "old_access_token",
            RefreshToken = "old_refresh_token"
        };

        var email = "test@example.com";

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, email)
        }));

        var cachedAuth = new LoginResponseDto
        {
            RefreshToken = command.RefreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(1)
        };

        var user = AppUser.Create(email: email);
        var claims = new List<Claim>();
        var newAccessToken = "new_access_token";
        var newRefreshToken = "new_refresh_token";
        var newRefreshTokenExpiry = DateTime.UtcNow.AddDays(1);

        var responseDto = new RefreshTokenResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryTime = newRefreshTokenExpiry
        };

        _tokenServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(command.AccessToken))
            .Returns(principal);

        _cacheServiceMock.Setup(x => x.GetAsync<LoginResponseDto>($"User:{email}:Token:AuthenticatedToken", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedAuth);

        _mockUserManager.Setup(x => x.FindByEmailAsync(email))
            .ReturnsAsync(user);

        _claimsTransformationMock.Setup(x => x.TransformClaims(user))
            .ReturnsAsync(claims);

        _tokenServiceMock.Setup(x => x.GenerateAccessToken(claims))
            .Returns(newAccessToken);

        _tokenServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns(newRefreshToken);

        _cacheServiceMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<RefreshTokenResponseDto>(), null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);
        result.Value.RefreshTokenExpiryTime = newRefreshTokenExpiry;

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(responseDto);
    }

    [Fact]
    public async Task Register_ShouldThrowInvalidAccessTokenException_NullAccessToken()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            AccessToken = null,
            RefreshToken = "old_refresh_token"
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Token.InvalidAccessToken>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Register_ShouldThrowNotFoundInCachedException_NoEmailInToken()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            AccessToken = "old_access_token",
            RefreshToken = "old_refresh_token"
        };

        var principal = new ClaimsPrincipal(new ClaimsIdentity());

        _tokenServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(command.AccessToken))
            .Returns(principal);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Token.NotFoundInCached>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Register_ShouldThrowNotFoundInCachedException_NullCachedAuth()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            AccessToken = "old_access_token",
            RefreshToken = "old_refresh_token"
        };

        var email = "test@example.com";

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, email)
        }));

        _tokenServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(command.AccessToken))
            .Returns(principal);

        _cacheServiceMock.Setup(x => x.GetAsync<LoginResponseDto>($"User:{email}:Token:AuthenticatedToken", It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDto)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Token.NotFoundInCached>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Register_ShouldThrowNotFoundInCachedException_MismatchedRefreshToken()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            AccessToken = "old_access_token",
            RefreshToken = "old_refresh_token"
        };

        var email = "test@example.com";

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, email)
        }));

        var cachedAuth = new LoginResponseDto
        {
            RefreshToken = "different_refresh_token",
            RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(1)
        };

        _tokenServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(command.AccessToken))
            .Returns(principal);

        _cacheServiceMock.Setup(x => x.GetAsync<LoginResponseDto>($"User:{email}:Token:AuthenticatedToken", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedAuth);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Token.NotFoundInCached>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Register_ShouldThrowNotFoundInCachedException_ExpiredRefreshToken()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            AccessToken = "old_access_token",
            RefreshToken = "old_refresh_token"
        };

        var email = "test@example.com";

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, email)
        }));

        var cachedAuth = new LoginResponseDto
        {
            RefreshToken = command.RefreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(-1)
        };

        _tokenServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(command.AccessToken))
            .Returns(principal);

        _cacheServiceMock.Setup(x => x.GetAsync<LoginResponseDto>($"User:{email}:Token:AuthenticatedToken", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedAuth);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Token.NotFoundInCached>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Register_ShouldThrowNotFoundException_UserNotFound()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            AccessToken = "old_access_token",
            RefreshToken = "old_refresh_token"
        };

        var email = "test@example.com";

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, email)
        }));

        var cachedAuth = new LoginResponseDto
        {
            RefreshToken = command.RefreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(1)
        };

        _tokenServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(command.AccessToken))
            .Returns(principal);

        _cacheServiceMock.Setup(x => x.GetAsync<LoginResponseDto>($"User:{email}:Token:AuthenticatedToken", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedAuth);

        _mockUserManager.Setup(x => x.FindByEmailAsync(email))
            .ReturnsAsync((AppUser)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.NotFound>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }
}
