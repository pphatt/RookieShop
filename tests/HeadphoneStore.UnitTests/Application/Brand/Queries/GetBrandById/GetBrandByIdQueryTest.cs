using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Brand.GetBrandById;
using HeadphoneStore.Shared.Services.Brand.GetById;

namespace HeadphoneStore.UnitTests.Application.Brand.Queries.GetBrandById;

[Trait("Brand", "GetBrandById")]
public class GetBrandByIdQueryTests : BaseTest
{
    [Fact]
    public void GetBrandByIdQuery_ShouldInitializeCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var requestDto = new GetBrandByIdRequestDto
        {
            Id = id
        };

        // Act
        var query = _mapper.Map<GetBrandByIdQuery>(requestDto);

        // Assert
        query.Should().NotBeNull();
        query.Id.Should().Be(id);
        query.BypassCache.Should().BeFalse();
        query.CacheKey.Should().Be($"Brands:{nameof(GetBrandByIdQuery)}:{id}");
        query.SlidingExpirationInMinutes.Should().Be(-1);
        query.AbsoluteExpirationInMinutes.Should().Be(-1);
    }
}
