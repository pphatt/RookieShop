using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Identity.GetUserById;

namespace HeadphoneStore.UnitTests.Application.Identity.Queries.GetUserById;

[Trait("Authentication", "GetUserById")]
public class GetUserByIdQueryTests : BaseTest
{
    [Fact]
    public void GetUserByIdQuery_ShouldInitializeCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var query = new GetUserByIdQuery
        {
            Id = userId
        };

        // Assert
        query.Should().NotBeNull();
        query.Id.Should().Be(userId);
    }
}
