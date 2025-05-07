using FluentAssertions;

using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.UseCases.V1.Identity.WhoAmI;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Dtos.Identity.User;

using Microsoft.AspNetCore.Identity;

using MockQueryable.Moq;

using Moq;

namespace HeadphoneStore.UnitTests.Application.Identity.Queries.WhoAmI;

using Exceptions = HeadphoneStore.Domain.Exceptions.Exceptions;

[Trait("Authentication", "WhoAmI")]
public class WhoAmIQueryHandlerTests : BaseTest
{
    private readonly Mock<UserManager<AppUser>> _mockUserManager;
    private readonly Mock<RoleManager<AppRole>> _mockRoleManager;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly WhoAmIQueryHandler _queryHandler;

    public WhoAmIQueryHandlerTests()
    {
        _mockUserManager = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
        _mockRoleManager = new Mock<RoleManager<AppRole>>(
            Mock.Of<IRoleStore<AppRole>>(), null, null, null, null);
        _mockCacheService = new Mock<ICacheService>();

        _queryHandler = new WhoAmIQueryHandler(
            _mockUserManager.Object,
            _mockRoleManager.Object,
            _mockCacheService.Object);
    }

    [Fact]
    public async Task WhoAmI_ShouldReturnSuccessResult_ValidUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new WhoAmIQuery { UserId = userId.ToString() };
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
        user.IsDeleted = false;
        user.CreatedDateTime = DateTimeOffset.UtcNow.AddDays(-10);
        user.UpdatedDateTime = DateTimeOffset.UtcNow;
        user.UserRoles = new List<IdentityUserRole<Guid>> { new IdentityUserRole<Guid> { RoleId = Guid.NewGuid() } };
        user.GetType().GetField("_addresses", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(user, new List<UserAddress>
            {
                new UserAddress { Address = "123 Main St", Street = "Main", Province = "CA", PhoneNumber = "1234567890" }
            });

        var role = new AppRole
        {
            Id = user.UserRoles.First().RoleId,
            Name = "User",
            NormalizedName = "USER",
            DisplayName = "Standard User",
            Status = RoleStatus.Active
        };

        var users = new List<AppUser> { user }.AsQueryable().BuildMockDbSet();
        var roles = new List<AppRole> { role }.AsQueryable().BuildMockDbSet();

        _mockUserManager.Setup(x => x.Users).Returns(users.Object);
        _mockRoleManager.Setup(x => x.Roles).Returns(roles.Object);
        _mockCacheService.Setup(x => x.SetAsync($"User:{user.Email}:Profile:WhoAmI", It.IsAny<UserDto>(), null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

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
        result.Value.CreatedDateTime.Should().Be(user.CreatedDateTime);
        result.Value.UpdatedDateTime.Should().Be(user.UpdatedDateTime);
        result.Value.UserAddress.Should().HaveCount(1);
        result.Value.UserAddress.First().Address.Should().Be(user.Addresses.First().Address);
        result.Value.Roles.Id.Should().Be(role.Id);
        result.Value.Roles.Name.Should().Be(role.Name);
        result.Value.Roles.RoleStatus.Should().Be("Active");
        _mockCacheService.Verify(x => x.SetAsync($"User:{user.Email}:Profile:WhoAmI", It.IsAny<UserDto>(), null, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task WhoAmI_ShouldThrowNotFoundException_UserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var query = new WhoAmIQuery { UserId = userId };

        var users = new List<AppUser>().AsQueryable().BuildMockDbSet();
        var roles = new List<AppRole>().AsQueryable().BuildMockDbSet();

        _mockUserManager.Setup(x => x.Users).Returns(users.Object);
        _mockRoleManager.Setup(x => x.Roles).Returns(roles.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.NotFound>(
            () => _queryHandler.Handle(query, CancellationToken.None));
    }
}
