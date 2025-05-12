using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Identity.GetAllUsersPaged;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumerations;

using Microsoft.AspNetCore.Identity;

using MockQueryable.Moq;

using Moq;

namespace HeadphoneStore.UnitTests.Application.Identity.Queries.GetAllUsersPaged;

[Trait("Authentication", "GetAllUserPaged")]
public class GetAllUserPagedQueryHandlerTests : BaseTest
{
    private readonly Mock<UserManager<AppUser>> _mockUserManager;
    private readonly Mock<RoleManager<AppRole>> _mockRoleManager;
    private readonly GetAllUsersPagedQueryHandler _queryHandler;

    public GetAllUserPagedQueryHandlerTests()
    {
        _mockUserManager = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

        _mockRoleManager = new Mock<RoleManager<AppRole>>(
            Mock.Of<IRoleStore<AppRole>>(), null, null, null, null);

        _queryHandler = new GetAllUsersPagedQueryHandler(_mockUserManager.Object, _mockRoleManager.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFilteredUsers_WhenSearchTermMatches()
    {
        // Arrange
        var userRoleId = Guid.NewGuid();
        var adminRoleId = Guid.NewGuid();
        var query = new GetAllUserPagedQuery
        {
            SearchTerm = "user1",
            PageIndex = 1,
            PageSize = 10
        };

        var user1 = AppUser.Create("user1@example.com", "User1", "One", "1234567890");
        user1.Id = Guid.NewGuid();
        user1.Status = EntityStatus.Active;
        user1.UserRoles = new List<IdentityUserRole<Guid>> { new IdentityUserRole<Guid> { RoleId = userRoleId } };

        var user2 = AppUser.Create("user2@example.com", "User2", "Two", "0987654321");
        user2.Id = Guid.NewGuid();
        user1.Status = EntityStatus.Active;
        user2.UserRoles = new List<IdentityUserRole<Guid>> { new IdentityUserRole<Guid> { RoleId = userRoleId } };

        var users = new List<AppUser> { user1, user2 }.AsQueryable().BuildMockDbSet();
        var roles = new List<AppRole>
        {
            new AppRole { Id = userRoleId, Name = "User", NormalizedName = "USER", DisplayName = "Customer" },
            new AppRole { Id = adminRoleId, Name = "admin", NormalizedName = "ADMIN", DisplayName = "Administrator" }
        }.AsQueryable().BuildMockDbSet();

        _mockUserManager.Setup(x => x.Users).Returns(users.Object);
        _mockRoleManager.Setup(x => x.Roles).Returns(roles.Object);

        // Act
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value.Items.Should().HaveCount(1);
        result.Value.TotalCount.Should().Be(1);
        result.Value.Items.First().Email.Should().Be("user1@example.com");
        result.Value.Items.First().Status.Should().Be("Active");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUsersMatchSearchTerm()
    {
        // Arrange
        var userRoleId = Guid.NewGuid();
        var adminRoleId = Guid.NewGuid();
        var query = new GetAllUserPagedQuery
        {
            SearchTerm = "nonexistent",
            PageIndex = 1,
            PageSize = 10
        };

        var user = AppUser.Create("user@example.com", "User", "One", "1234567890");
        user.Id = Guid.NewGuid();
        user.Status = EntityStatus.Active;
        user.UserRoles = new List<IdentityUserRole<Guid>> { new IdentityUserRole<Guid> { RoleId = userRoleId } };

        var users = new List<AppUser> { user }.AsQueryable().BuildMockDbSet();
        var roles = new List<AppRole>
        {
            new AppRole { Id = userRoleId, Name = "User", NormalizedName = "USER", DisplayName = "Customer" },
            new AppRole { Id = adminRoleId, Name = "admin", NormalizedName = "ADMIN", DisplayName = "Administrator" }
        }.AsQueryable().BuildMockDbSet();

        _mockUserManager.Setup(x => x.Users).Returns(users.Object);
        _mockRoleManager.Setup(x => x.Roles).Returns(roles.Object);

        // Act
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value.Items.Should().BeEmpty();
        result.Value.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldReturnSecondPage_WhenPaginated()
    {
        // Arrange
        var userRoleId = Guid.NewGuid();
        var adminRoleId = Guid.NewGuid();
        var query = new GetAllUserPagedQuery
        {
            PageIndex = 2,
            PageSize = 1
        };

        var user1 = AppUser.Create("user1@example.com", "User1", "One", "1234567890");
        user1.Id = Guid.NewGuid();
        user1.Status = EntityStatus.Active;
        user1.UserRoles = new List<IdentityUserRole<Guid>> { new IdentityUserRole<Guid> { RoleId = userRoleId } };

        var user2 = AppUser.Create("user2@example.com", "User2", "Two", "0987654321");
        user2.Id = Guid.NewGuid();
        user2.Status = EntityStatus.Active;
        user2.UserRoles = new List<IdentityUserRole<Guid>> { new IdentityUserRole<Guid> { RoleId = userRoleId } };

        var users = new List<AppUser> { user1, user2 }.AsQueryable().BuildMockDbSet();
        var roles = new List<AppRole>
        {
            new AppRole { Id = userRoleId, Name = "User", NormalizedName = "USER", DisplayName = "Customer" },
            new AppRole { Id = adminRoleId, Name = "admin", NormalizedName = "ADMIN", DisplayName = "Administrator" }
        }.AsQueryable().BuildMockDbSet();

        _mockUserManager.Setup(x => x.Users).Returns(users.Object);
        _mockRoleManager.Setup(x => x.Roles).Returns(roles.Object);

        // Act
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value.Items.Should().HaveCount(1);
        result.Value.TotalCount.Should().Be(2);
        result.Value.PageIndex.Should().Be(2);
        result.Value.PageSize.Should().Be(1);
        result.Value.Items.First().Email.Should().Be("user2@example.com");
    }
}
