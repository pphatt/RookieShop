using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Identity.GetUserById;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumerations;

using Microsoft.AspNetCore.Identity;

using Moq;

namespace HeadphoneStore.UnitTests.Application.Identity.Queries.GetUserById;

using Exceptions = Domain.Exceptions.Exceptions;

[Trait("Authentication", "GetUserById")]
public class GetUserByIdQueryHandlerTests : BaseTest
{
    private readonly Mock<UserManager<AppUser>> _mockUserManager;
    private readonly GetUserByIdQueryHandler _queryHandler;

    public GetUserByIdQueryHandlerTests()
    {
        _mockUserManager = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
        _queryHandler = new GetUserByIdQueryHandler(_mockUserManager.Object);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnUserDto_WhenUserExistsAndIsActive()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserByIdQuery { Id = userId };
        var user = AppUser.Create(
            email: "test@example.com",
            firstName: "John",
            lastName: "Doe",
            phoneNumber: "1234567890");
        user.Id = userId;
        user.DayOfBirth = new DateTimeOffset(new DateTime(1990, 1, 1));
        user.Avatar = "avatar.png";
        user.Status = EntityStatus.Active;
        user.IsActive = true;

        _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        // Act
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(user.Id);
        result.Value.FirstName.Should().Be(user.FirstName);
        result.Value.LastName.Should().Be(user.LastName);
        result.Value.Email.Should().Be(user.Email);
        result.Value.PhoneNumber.Should().Be(user.PhoneNumber);
        result.Value.DayOfBirth.Should().Be(user.DayOfBirth);
        result.Value.Avatar.Should().Be(user.Avatar);
        result.Value.Status.Should().Be("True");
        result.Value.Roles.DisplayName.Should().Be("User");
    }

    [Fact]
    public async Task GetUserById_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserByIdQuery { Id = userId };

        _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync((AppUser?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.NotFound>(
            () => _queryHandler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task GetUserById_ShouldThrowInactiveOrLockedOutException_WhenUserIsInactive()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserByIdQuery { Id = userId };
        var user = AppUser.Create(
            email: "test@example.com",
            firstName: "John",
            lastName: "Doe",
            phoneNumber: "1234567890");
        user.Id = userId;
        user.Status = EntityStatus.Inactive;
        user.IsActive = false;

        _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.InactiveOrLockedOut>(
            () => _queryHandler.Handle(query, CancellationToken.None));
    }
}
