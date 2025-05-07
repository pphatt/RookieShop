using System.Security.Claims;

using FluentAssertions;

using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.UseCases.V1.Identity.Logout;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Services.Identity.Login;

using Microsoft.Extensions.Logging;

using Moq;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.Logout;

using Exceptions = Domain.Exceptions.Exceptions;

[Trait("Authentication", "Logout")]
public class LogoutCommandHandlerTests : BaseTest
{
    private readonly LogoutCommandHandler _commandHandler;

    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<ILogger<LogoutCommandHandler>> _mockLogger;

    public LogoutCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockTokenService = new Mock<ITokenService>();
        _mockCacheService = new Mock<ICacheService>();
        _mockLogger = new Mock<ILogger<LogoutCommandHandler>>();

        _commandHandler = new LogoutCommandHandler(
            _mockUserRepository.Object,
            _mockTokenService.Object,
            _mockCacheService.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Logout_ShouldReturnSuccessResult_ValidToken()
    {
        // Arrange
        var command = new LogoutCommand { AccessToken = "some_access_token" };

        var email = "test@example.com";

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, email)
        }));

        var cachedAuth = new LoginResponseDto
        {
            RefreshToken = "some_refresh_token",
            RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(1)
        };

        _mockTokenService.Setup(x => x.GetPrincipalFromExpiredToken(command.AccessToken))
            .Returns(principal);

        _mockCacheService.Setup(x => x.GetAsync<LoginResponseDto>($"User:{email}:Token:AuthenticatedToken", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedAuth);

        _mockCacheService.Setup(x => x.RemoveAsync($"User:{email}:Token:AuthenticatedToken", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUserRepository.Setup(x => x.UpdateLastLogin(email))
            .ReturnsAsync(true);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public async Task Logout_ShouldThrowEmailKeyNotFoundException_NoEmailInToken()
    {
        // Arrange
        var command = new LogoutCommand { AccessToken = "some_access_token" };

        var principal = new ClaimsPrincipal(new ClaimsIdentity());

        _mockTokenService.Setup(x => x.GetPrincipalFromExpiredToken(command.AccessToken))
            .Returns(principal);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Token.EmailKeyNotFound>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Logout_ShouldThrowNotFoundInCachedException_NullCachedAuthToken()
    {
        // Arrange
        var command = new LogoutCommand { AccessToken = "some_access_token" };

        var email = "test@example.com";

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, email)
        }));

        _mockTokenService.Setup(x => x.GetPrincipalFromExpiredToken(command.AccessToken))
            .Returns(principal);

        _mockCacheService.Setup(x => x.GetAsync<LoginResponseDto>($"User:{email}:Token:AuthenticatedToken", It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponseDto)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Token.NotFoundInCached>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }
}
