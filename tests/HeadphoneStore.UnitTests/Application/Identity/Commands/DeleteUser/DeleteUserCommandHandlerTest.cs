using FluentAssertions;

using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.UseCases.V1.Identity.DeleteUser;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Constraints;
using HeadphoneStore.Domain.Exceptions;

using Microsoft.AspNetCore.Identity;

using Moq;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.DeleteUser;

[Trait("Authentication", "DeleteUser")]
public class DeleteUserCommandHandlerTests : BaseTest
{
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly DeleteUserCommandHandler _commandHandler;

    public DeleteUserCommandHandlerTests()
    {
        _mockCacheService = new Mock<ICacheService>();

        _commandHandler = new DeleteUserCommandHandler(
            _mockUserManager.Object,
            _mockCacheService.Object);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnSuccessResult_ValidInput()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = Guid.NewGuid() };

        var user = new AppUser { Id = command.Id, Email = "test@example.com" };

        var roles = new List<string> { "User" };

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _mockUserManager.Setup(x => x.RemoveFromRolesAsync(user, roles))
            .ReturnsAsync(IdentityResult.Success);

        _mockCacheService.Setup(x => x.RemoveAsync($"User:{user.Email}:Token:AuthenticatedToken", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUserManager.Setup(x => x.DeleteAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Be("Delete user successfully.");
    }

    [Fact]
    public async Task DeleteUser_ShouldThrowNotFoundException_UserNotFound()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = Guid.NewGuid() };

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync((AppUser)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.NotFound>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteUser_ShouldThrowNotFoundException_NoRoles()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = Guid.NewGuid() };

        var user = new AppUser { Id = command.Id, Email = "test@example.com" };

        var roles = new List<string>();

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Role.NotFound>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteUser_ShouldThrowCannotDeleteException_AdminUser()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = Guid.NewGuid() };

        var user = new AppUser { Id = command.Id, Email = "test@example.com" };

        var roles = new List<string> { Roles.Admin };

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.CannotDelete>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteUser_ShouldThrowCustomException_FailedRoleRemoval()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = Guid.NewGuid() };

        var user = new AppUser { Id = command.Id, Email = "test@example.com" };

        var roles = new List<string> { "User" };

        var identityError = new IdentityError { Code = "RoleError", Description = "Role removal failed." };

        var failedResult = IdentityResult.Failed(identityError);

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _mockUserManager.Setup(x => x.RemoveFromRolesAsync(user, roles))
            .ReturnsAsync(failedResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(
            () => _commandHandler.Handle(command, CancellationToken.None));
        exception.Code.Should().Be("RoleError");
        exception.Description.Should().Be("Role removal failed.");
        exception.Type.Should().Be(ExceptionType.Forbidden);
    }

    [Fact]
    public async Task DeleteUser_ShouldThrowCustomException_FailedUserDeletion()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = Guid.NewGuid() };

        var user = new AppUser { Id = command.Id, Email = "test@example.com" };

        var roles = new List<string> { "User" };

        var identityError = new IdentityError { Code = "DeleteError", Description = "User deletion failed." };

        var failedResult = IdentityResult.Failed(identityError);

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _mockUserManager.Setup(x => x.RemoveFromRolesAsync(user, roles))
            .ReturnsAsync(IdentityResult.Success);

        _mockCacheService.Setup(x => x.RemoveAsync($"User:{user.Email}:Token:AuthenticatedToken", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUserManager.Setup(x => x.DeleteAsync(user))
            .ReturnsAsync(failedResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(
            () => _commandHandler.Handle(command, CancellationToken.None));
        exception.Code.Should().Be("DeleteError");
        exception.Description.Should().Be("User deletion failed.");
        exception.Type.Should().Be(ExceptionType.Unexpected);
    }
}
