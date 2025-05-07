using FluentAssertions;
using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.UseCases.V1.Identity.Login;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Services.Identity.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.Login;

using Exceptions = Domain.Exceptions.Exceptions;

[Trait("Authentication", "Login")]
public class LoginCommandHandlerTest : BaseTest
{
    private readonly LoginCommandHandler _commandHandler;

    private readonly Mock<ILogger<LoginCommandHandler>> _mockLogger;
    private readonly Mock<SignInManager<AppUser>> _mockSignInManager;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IClaimsTransformation> _mockClaimsTransformation;
    private readonly Mock<ICacheService> _mockCacheService;

    public LoginCommandHandlerTest()
    {
        _mockLogger = new Mock<ILogger<LoginCommandHandler>>();

        _mockSignInManager = new Mock<SignInManager<AppUser>>(
            _mockUserManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
            null, null, null, null);

        _mockTokenService = new Mock<ITokenService>();

        _mockClaimsTransformation = new Mock<IClaimsTransformation>();

        _mockCacheService = new Mock<ICacheService>();

        _commandHandler = new LoginCommandHandler(
            _mockLogger.Object,
            _mockSignInManager.Object,
            _mockTokenService.Object,
            _mockUserManager.Object,
            _mockClaimsTransformation.Object,
            _mockCacheService.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnSuccessResult_ValidCredentials()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "Password123!");
        var user = AppUser.Create(email: command.Email);
        var claims = new List<Claim>();
        var accessToken = "access_token";
        var refreshToken = "refresh_token";
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(1);

        var responseDto = new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiry
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(user, command.Password, false))
            .ReturnsAsync(SignInResult.Success);

        _mockClaimsTransformation.Setup(x => x.TransformClaims(user))
            .ReturnsAsync(claims);

        _mockTokenService.Setup(x => x.GenerateAccessToken(claims))
            .Returns(accessToken);

        _mockTokenService.Setup(x => x.GenerateRefreshToken())
            .Returns(refreshToken);

        _mockCacheService.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<LoginResponseDto>(), null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);
        result.Value.RefreshTokenExpiryTime = refreshTokenExpiry;

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(responseDto);
    }

    [Fact]
    public async Task Login_ShouldThrowNotFoundException_UserNotFound()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "Password123!");

        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((AppUser)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.NotFound>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Login_ShouldThrowInactiveOrLockedOutException_InactiveUser()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "Password123!");

        var user = AppUser.Create(email: command.Email);
        user.IsActive = false;

        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.InactiveOrLockedOut>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Login_ShouldThrowInvalidCredentialsException_InvalidCredentials()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "Password123!");

        var user = AppUser.Create(email: command.Email);
        user.IsActive = true;

        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(user, command.Password, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.InvalidCredentials>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }
}
