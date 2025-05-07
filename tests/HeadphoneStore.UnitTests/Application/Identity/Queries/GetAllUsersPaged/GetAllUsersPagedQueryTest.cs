using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Identity.GetAllUserPaged;
using HeadphoneStore.Shared.Services.Identity.GetAllUserPaged;

namespace HeadphoneStore.UnitTests.Application.Identity.Queries.GetAllUsersPaged;

[Trait("Authentication", "GetAllUserPaged")]
public class GetAllUserPagedQueryTests : BaseTest
{
    [Fact]
    public void GetAllUserPagedQuery_ShouldInitializeCorrectly()
    {
        // Arrange
        var searchTerm = "test";
        var pageIndex = 2;
        var pageSize = 20;
        var requestDto = new GetAllUserPagedRequestDto
        {
            SearchTerm = searchTerm,
            PageIndex = pageIndex,
            PageSize = pageSize
        };

        // Act
        var query = new GetAllUserPagedQuery
        {
            SearchTerm = searchTerm,
            PageIndex = pageIndex,
            PageSize = pageSize
        };

        // Assert
        query.Should().NotBeNull();
        query.SearchTerm.Should().Be(searchTerm);
        query.PageIndex.Should().Be(pageIndex);
        query.PageSize.Should().Be(pageSize);
    }
}
