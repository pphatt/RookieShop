using FluentAssertions;

using HeadphoneStore.Application.Abstracts.Interface.Services.Mail;
using HeadphoneStore.Application.UseCases.V1.Identity.CreateUser;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Exceptions;

using Microsoft.AspNetCore.Identity;

using Moq;

namespace HeadphoneStore.UnitTests.Application.Identity.Commands.CreateUser;

[Trait("Authentication", "CreateUser")]
public class CreateUserCommandHandlerTests : BaseTest
{
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateUserCommandHandler _commandHandler;

    public CreateUserCommandHandlerTests()
    {
        _mockEmailService = new Mock<IEmailService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _commandHandler = new CreateUserCommandHandler(
            _mockUserManager.Object,
            _mockRoleManager.Object,
            _mockEmailService.Object,
            _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnSuccessResult_ValidInput()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };

        var user = AppUser.Create(command.Email, command.FirstName, command.LastName, command.PhoneNumber);

        var role = new AppRole { Name = "User" };

        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((AppUser)null);

        _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId.ToString()))
            .ReturnsAsync(role);

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), role.Name))
            .ReturnsAsync(IdentityResult.Success);

        _mockEmailService.Setup(x => x.SendEmailAsync(It.IsAny<EmailContent>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Be("Create new user successfully.");
    }

    [Fact]
    public async Task CreateUser_ShouldThrowDuplicateEmailException_ExistingEmail()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };

        var existingUser = AppUser.Create(command.Email, command.FirstName, command.LastName, command.PhoneNumber);

        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.DuplicateEmail>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task CreateUser_ShouldThrowNotFoundException_NonExistentRole()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((AppUser)null);

        _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId.ToString()))
            .ReturnsAsync((AppRole)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Role.NotFound>(
            () => _commandHandler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task CreateUser_ShouldThrowCustomException_FailedUserCreation()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };

        var role = new AppRole { Name = "User" };

        var identityError = new IdentityError { Code = "InvalidUser", Description = "User creation failed." };

        var failedResult = IdentityResult.Failed(identityError);

        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((AppUser)null);

        _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId.ToString()))
            .ReturnsAsync(role);

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(failedResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(
            () => _commandHandler.Handle(command, CancellationToken.None));

        exception.Code.Should().Be("InvalidUser");
        exception.Description.Should().Be("User creation failed.");
        exception.Type.Should().Be(ExceptionType.Validation);
    }

    [Fact]
    public async Task CreateUser_ShouldThrowCustomException_FailedRoleAssignment()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            RoleId = Guid.NewGuid(),
            Status = "Active"
        };

        var role = new AppRole { Name = "User" };

        var identityError = new IdentityError { Code = "InvalidRole", Description = "Role assignment failed." };

        var failedResult = IdentityResult.Failed(identityError);

        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((AppUser)null);

        _mockRoleManager.Setup(x => x.FindByIdAsync(command.RoleId.ToString()))
            .ReturnsAsync(role);

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), role.Name))
            .ReturnsAsync(failedResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(
            () => _commandHandler.Handle(command, CancellationToken.None));
        exception.Code.Should().Be("InvalidRole");
        exception.Description.Should().Be("Role assignment failed.");
        exception.Type.Should().Be(ExceptionType.Validation);
    }
}
