using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsPaged;

namespace HeadphoneStore.UnitTests.Application.Brand.Queries.GetAllBrandsPaged;

[Trait("Brand", "GetAllBrandsPaged")]
public class GetAllBrandsPagedQueryTests : BaseTest
{
    [Fact]
    public void GetAllBrandsPagedQuery_ShouldInitializeCorrectly()
    {
        // Arrange
        var searchTerm = "Sony";
        var pageIndex = 1;
        var pageSize = 10;

        // Act
        var query = new GetAllBrandsPagedQuery
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
        query.BypassCache.Should().BeFalse();
        query.CacheKey.Should().Be($"Brands:{nameof(GetAllBrandsPagedQuery)}:SearchTerm:{searchTerm}:Page:{pageIndex}:{pageSize}");
        query.SlidingExpirationInMinutes.Should().Be(-1);
        query.AbsoluteExpirationInMinutes.Should().Be(-1);
    }

    [Fact]
    public void GetAllBrandsPagedQuery_ShouldGenerateCorrectCacheKey_WhenSearchTermIsNull()
    {
        // Arrange
        var pageIndex = 1;
        var pageSize = 10;

        // Act
        var query = new GetAllBrandsPagedQuery
        {
            SearchTerm = null,
            PageIndex = pageIndex,
            PageSize = pageSize
        };

        // Assert
        query.CacheKey.Should().Be($"Brands:{nameof(GetAllBrandsPagedQuery)}:Page:{pageIndex}:{pageSize}");
    }
}
