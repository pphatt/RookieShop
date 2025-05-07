using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Identity.UpdateUser;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Domain.Exceptions;

using Microsoft.AspNetCore.Identity;

using Moq;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.UpdateUser;

using Exceptions = Exceptions;

[Trait("Authentication", "UpdateUser")]
public class UpdateUserCommandHandlerTests : BaseTest
{
    private readonly Mock<UserManager<AppUser>> _mockUserManager;
    private readonly Mock<RoleManager<AppRole>> _mockRoleManager;
    private readonly UpdateUserCommandHandler _commandHandler;

    public UpdateUserCommandHandlerTests()
    {
        _mockUserManager = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
        _mockRoleManager = new Mock<RoleManager<AppRole>>(
            Mock.Of<IRoleStore<AppRole>>(), null, null, null, null);

        _commandHandler = new UpdateUserCommandHandler(
            _mockUserManager.Object,
            _mockRoleManager.Object);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnSuccessResult_ValidInput()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };

        var user = new AppUser
        {
            Id = command.Id,
            FirstName = "OldFirst",
            LastName = "OldLast",
            PhoneNumber = "0987654321",
            Status = EntityStatus.Inactive
        };
        var role = new AppRole { Name = "User" };
        var currentRoles = new List<string> { "OldRole" };

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync(user);

        _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId.ToString()))
            .ReturnsAsync(role);

        _mockUserManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(currentRoles);

        _mockUserManager.Setup(x => x.RemoveFromRolesAsync(user, currentRoles))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager.Setup(x => x.AddToRoleAsync(user, role.Name))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager.Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Be("Update user successfully.");
        user.FirstName.Should().Be(command.FirstName);
        user.LastName.Should().Be(command.LastName);
        user.PhoneNumber.Should().Be(command.PhoneNumber);
        user.Status.Should().Be(EntityStatus.Active);
    }

    [Fact]
    public async Task UpdateUser_ShouldThrowNotFoundException_UserNotFound()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync((AppUser)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.NotFound>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateUser_ShouldThrowNotFoundException_NonExistentRole()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };
        var user = new AppUser { Id = command.Id };

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync(user);

        _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId.ToString()))
            .ReturnsAsync((AppRole)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Role.NotFound>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateUser_ShouldThrowCustomException_FailedRoleRemoval()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };
        var user = new AppUser { Id = command.Id };
        var role = new AppRole { Name = "User" };
        var currentRoles = new List<string> { "OldRole" };
        var identityError = new IdentityError { Code = "RoleError", Description = "Role removal failed." };
        var failedResult = IdentityResult.Failed(identityError);

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync(user);

        _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId.ToString()))
            .ReturnsAsync(role);

        _mockUserManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(currentRoles);

        _mockUserManager.Setup(x => x.RemoveFromRolesAsync(user, currentRoles))
            .ReturnsAsync(failedResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(
            () => _commandHandler.Handle(command, CancellationToken.None));
        exception.Code.Should().Be("RoleError");
        exception.Description.Should().Be("Role removal failed.");
        exception.Type.Should().Be(ExceptionType.Validation);
    }

    [Fact]
    public async Task UpdateUser_ShouldThrowCustomException_FailedRoleAddition()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };
        var user = new AppUser { Id = command.Id };
        var role = new AppRole { Name = "User" };
        var currentRoles = new List<string> { "OldRole" };
        var identityError = new IdentityError { Code = "RoleError", Description = "Role addition failed." };
        var failedResult = IdentityResult.Failed(identityError);

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync(user);

        _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId.ToString()))
            .ReturnsAsync(role);

        _mockUserManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(currentRoles);

        _mockUserManager.Setup(x => x.RemoveFromRolesAsync(user, currentRoles))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager.Setup(x => x.AddToRoleAsync(user, role.Name))
            .ReturnsAsync(failedResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(
            () => _commandHandler.Handle(command, CancellationToken.None));
        exception.Code.Should().Be("RoleError");
        exception.Description.Should().Be("Role addition failed.");
        exception.Type.Should().Be(ExceptionType.Validation);
    }

    [Fact]
    public async Task UpdateUser_ShouldThrowCustomException_FailedUserUpdate()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };
        var user = new AppUser { Id = command.Id };
        var role = new AppRole { Name = "User" };
        var currentRoles = new List<string> { "OldRole" };
        var identityError = new IdentityError { Code = "UpdateError", Description = "User update failed." };
        var failedResult = IdentityResult.Failed(identityError);

        _mockUserManager.Setup(x => x.FindByIdAsync(command.Id.ToString()))
            .ReturnsAsync(user);

        _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId.ToString()))
            .ReturnsAsync(role);

        _mockUserManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(currentRoles);

        _mockUserManager.Setup(x => x.RemoveFromRolesAsync(user, currentRoles))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager.Setup(x => x.AddToRoleAsync(user, role.Name))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager.Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(failedResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(
            () => _commandHandler.Handle(command, CancellationToken.None));
        exception.Code.Should().Be("UpdateError");
        exception.Description.Should().Be("User update failed.");
        exception.Type.Should().Be(ExceptionType.Unexpected);
    }
}
