using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Identity.WhoAmI;

namespace HeadphoneStore.UnitTests.Application.Identity.Queries.WhoAmI;

[Trait("Authentication", "WhoAmI")]
public class WhoAmIQueryTests : BaseTest
{
    [Fact]
    public void WhoAmIQuery_MapCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();

        // Act
        var query = new WhoAmIQuery
        {
            UserId = userId
        };

        // Assert
        query.Should().NotBeNull();
        query.UserId.Should().Be(userId);
    }
}
