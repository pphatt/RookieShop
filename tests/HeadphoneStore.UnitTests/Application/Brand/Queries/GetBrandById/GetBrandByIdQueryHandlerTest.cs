using System.Linq.Expressions;

using FluentAssertions;

using HeadphoneStore.Application.UseCases.V1.Brand.GetBrandById;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Dtos.Brand;

using Moq;

namespace HeadphoneStore.UnitTests.Application.Brand.Queries.GetBrandById;

using Brand = Domain.Aggregates.Brands.Entities.Brand;
using Exceptions = HeadphoneStore.Domain.Exceptions.Exceptions;

[Trait("Brand", "GetBrandById")]
public class GetBrandByIdQueryHandlerTests : BaseTest
{
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly GetBrandByIdQueryHandler _handler;

    public GetBrandByIdQueryHandlerTests()
    {
        _mockBrandRepository = new Mock<IBrandRepository>();
        _handler = new GetBrandByIdQueryHandler(_mockBrandRepository.Object);
    }

    [Fact]
    public async Task GetBrandById_ShouldReturnBrandDto_WithValidId()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var query = new GetBrandByIdQuery { Id = brandId };
        var brand = Brand.Create("TestBrand", "test-slug", "Test description", EntityStatus.Active);
        brand.Id = brandId;

        _mockBrandRepository.Setup(x => x.FindByIdAsync(brandId,
                                                        It.IsAny<CancellationToken>(),
                                                        It.IsAny<Expression<Func<Brand, object>>[]>()))
            .ReturnsAsync(brand);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new BrandDto
        {
            Id = brand.Id,
            Name = brand.Name,
            Slug = brand.Slug,
            Description = brand.Description,
            Status = brand.Status.ToString()
        });
    }

    [Fact]
    public async Task GetBrandById_ShouldThrowBrandNotFoundException_WithNonExistentBrand()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var query = new GetBrandByIdQuery { Id = brandId };

        _mockBrandRepository.Setup(x => x.FindByIdAsync(brandId,
                                                        It.IsAny<CancellationToken>(),
                                                        It.IsAny<Expression<Func<Brand, object>>[]>()))
            .ReturnsAsync((Brand)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Brand.NotFound>(
            () => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task GetBrandById_ShouldThrowBrandNotFoundException_WithDeletedBrand()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var query = new GetBrandByIdQuery { Id = brandId };
        var brand = Brand.Create("TestBrand", "test-slug", "Test description", EntityStatus.Active);
        brand.Id = brandId;
        brand.IsDeleted = true;

        _mockBrandRepository.Setup(x => x.FindByIdAsync(brandId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
            .ReturnsAsync(brand);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Brand.NotFound>(
            () => _handler.Handle(query, CancellationToken.None));
    }
}
