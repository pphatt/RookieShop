using System.Linq.Expressions;

using FluentAssertions;

using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumerations;

using Microsoft.AspNetCore.Identity;

using Moq;

namespace HeadphoneStore.UnitTests.Application.Brand.Commands.UpdateBrand;

using Brand = Domain.Aggregates.Brands.Entities.Brand;
using Exceptions = Domain.Exceptions.Exceptions;

[Trait("Brand", "UpdateBrand")]
public class UpdateBrandCommandHandlerTests : BaseTest
{
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly UpdateBrandCommandHandler _handler;

    public UpdateBrandCommandHandlerTests()
    {
        _mockBrandRepository = new Mock<IBrandRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCacheService = new Mock<ICacheService>();
        _handler = new UpdateBrandCommandHandler(
            _mockBrandRepository.Object,
            _mockUnitOfWork.Object,
            _mockCacheService.Object);
    }

    [Fact]
    public async Task UpdateBrand_ShouldUpdateBrandSuccessfully_WithValidRequest()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var command = new UpdateBrandCommand
        {
            Id = brandId,
            Name = "UpdatedBrand",
            Slug = "updated-brand",
            Description = "Updated description",
            Status = "Active",
        };

        var user = new AppUser { Id = updatedBy };
        var brand = Brand.Create("OriginalBrand", "original-slug", "Original description", EntityStatus.Active);
        brand.Id = brandId;

        _mockUserManager.Setup(x => x.FindByIdAsync(updatedBy.ToString())).ReturnsAsync(user);
        _mockBrandRepository.Setup(x => x.FindByIdAsync(brandId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
            .ReturnsAsync(brand);
        _mockBrandRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Brand, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(new List<Brand>().AsQueryable());
        _mockBrandRepository.Setup(x => x.IsSlugAlreadyExisted(command.Slug, brandId)).ReturnsAsync(false);
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _mockCacheService.Setup(x => x.RemoveByPrefixAsync("Brands", It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Be("Update brand successfully.");
    }

    [Fact]
    public async Task UpdateBrand_ShouldThrowIdentityNotFoundException_WithNonExistentUser()
    {
        // Arrange
        var command = new UpdateBrandCommand
        {
            Id = Guid.NewGuid(),
            Name = "UpdatedBrand",
            Slug = "updated-brand",
            Description = "Updated description",
            Status = "Active",
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.NotFound>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonExistentBrand_ShouldThrowBrandNotFoundException()
    {
        // Arrange
        var command = new UpdateBrandCommand
        {
            Id = Guid.NewGuid(),
            Name = "UpdatedBrand",
            Slug = "updated-brand",
            Description = "Updated description",
            Status = "Active",
        };

        _mockBrandRepository.Setup(x => x.FindByIdAsync(command.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
            .ReturnsAsync((Brand)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Brand.NotFound>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateBrand_ShouldThrowDuplicateNameException_WithDuplicateName()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var command = new UpdateBrandCommand
        {
            Id = brandId,
            Name = "ExistingBrand",
            Slug = "updated-brand",
            Description = "Updated description",
            Status = "Active",
        };

        var brand = Brand.Create("OriginalBrand", "original-slug", "Original description", EntityStatus.Active);
        brand.Id = brandId;
        var existingBrand = Brand.Create("ExistingBrand", "existing-slug", "Existing description", EntityStatus.Active);
        existingBrand.Id = Guid.NewGuid();

        _mockBrandRepository.Setup(x => x.FindByIdAsync(brandId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
            .ReturnsAsync(brand);

        _mockBrandRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Brand, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(new List<Brand> { existingBrand }.AsQueryable());

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Brand.DuplicateName>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateBrand_ShouldThrowSlugExistsException_WithDuplicateSlug()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var command = new UpdateBrandCommand
        {
            Id = brandId,
            Name = "UpdatedBrand",
            Slug = "existing-slug",
            Description = "Updated description",
            Status = "Active",
        };

        var brand = Brand.Create("UpdatedBrand", "existing-slug", "Original description", EntityStatus.Active);
        brand.Id = brandId;

        _mockBrandRepository.Setup(x => x.FindByIdAsync(brandId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
            .ReturnsAsync(brand);

        _mockBrandRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Brand, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(new List<Brand>().AsQueryable());

        _mockBrandRepository.Setup(x => x.IsSlugAlreadyExisted(command.Slug, brandId)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Brand.SlugExists>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateBrand_ShouldThrowAlreadyDeletedException_WithDeletedBrand()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var command = new UpdateBrandCommand
        {
            Id = brandId,
            Name = "UpdatedBrand",
            Slug = "updated-brand",
            Description = "Updated description",
            Status = "Active",
        };

        var brand = Brand.Create("OriginalBrand", "original-slug", "Original description", EntityStatus.Active);
        brand.Id = brandId;
        brand.IsDeleted = true;

        _mockBrandRepository.Setup(x => x.FindByIdAsync(brandId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
            .ReturnsAsync(brand);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Brand.AlreadyDeleted>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}
