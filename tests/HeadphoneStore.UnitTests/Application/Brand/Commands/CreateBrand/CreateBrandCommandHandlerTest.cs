using System.Linq.Expressions;

using FluentAssertions;

using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;

using Moq;

namespace HeadphoneStore.Application.Tests.UseCases.V1.Brand;

using Brand = Domain.Aggregates.Products.Entities.Brand;

[Trait("Brand", "CreateBrand")]
public class CreateBrandCommandHandlerTests
{
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly CreateBrandCommandHandler _handler;

    public CreateBrandCommandHandlerTests()
    {
        _mockBrandRepository = new Mock<IBrandRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCacheService = new Mock<ICacheService>();
        _handler = new CreateBrandCommandHandler(
            _mockBrandRepository.Object,
            _mockUnitOfWork.Object,
            _mockCacheService.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCreateBrandSuccessfully()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = "Sony",
            Slug = "sony",
            Description = "Sony headphones brand",
            Status = "Active"
        };

        // Mock the FindByCondition correctly without using optional parameters
        _mockBrandRepository.Setup(x => x.FindByCondition(
            It.IsAny<Expression<Func<Brand, bool>>>(),
            It.IsAny<CancellationToken>()))
            .Returns(new List<Brand>().AsQueryable());

        // Setting up IsSlugAlreadyExisted with specific parameters to avoid optional parameter issue
        _mockBrandRepository.Setup(x => x.IsSlugAlreadyExisted(It.IsAny<string>(), null))
            .ReturnsAsync(false);

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockCacheService.Setup(x => x.RemoveByPrefixAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Be("Create new brand successfully.");

        _mockBrandRepository.Verify(x => x.Add(It.IsAny<Brand>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockCacheService.Verify(x => x.RemoveByPrefixAsync("Brands", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateName_ShouldThrowDuplicateNameException()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = "Sony",
            Slug = "sony",
            Description = "Sony headphones brand",
            Status = "Active"
        };

        var existingBrand = new List<Brand>
        {
            Brand.Create("Sony", "sony-different", "Existing Sony brand", Guid.NewGuid(), EntityStatus.Active)
        };

        _mockBrandRepository.Setup(x => x.FindByCondition(
            It.IsAny<Expression<Func<Brand, bool>>>(),
            It.IsAny<CancellationToken>()))
            .Returns(existingBrand.AsQueryable());

        // Act & Assert
        await Assert.ThrowsAsync<HeadphoneStore.Domain.Exceptions.Exceptions.Brand.DuplicateName>(
            () => _handler.Handle(command, CancellationToken.None));

        _mockBrandRepository.Verify(x => x.Add(It.IsAny<Brand>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mockCacheService.Verify(x => x.RemoveByPrefixAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithDuplicateSlug_ShouldThrowSlugExistsException()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = "New Sony",
            Slug = "sony",
            Description = "New Sony headphones brand",
            Status = "Active"
        };

        _mockBrandRepository.Setup(x => x.FindByCondition(
            It.IsAny<Expression<Func<Brand, bool>>>(),
            It.IsAny<CancellationToken>()))
            .Returns(new List<Brand>().AsQueryable());

        _mockBrandRepository.Setup(x => x.IsSlugAlreadyExisted("sony", null))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<HeadphoneStore.Domain.Exceptions.Exceptions.Brand.SlugExists>(
            () => _handler.Handle(command, CancellationToken.None));

        _mockBrandRepository.Verify(x => x.Add(It.IsAny<Brand>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mockCacheService.Verify(x => x.RemoveByPrefixAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidStatus_ShouldUseDefaultStatus()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = "Sony",
            Slug = "sony",
            Description = "Sony headphones brand",
            Status = "InvalidStatus" // This should default to the enum's default value
        };

        Brand capturedBrand = null;

        _mockBrandRepository.Setup(x => x.FindByCondition(
            It.IsAny<Expression<Func<Brand, bool>>>(),
            It.IsAny<CancellationToken>()))
            .Returns(new List<Brand>().AsQueryable());

        _mockBrandRepository.Setup(x => x.IsSlugAlreadyExisted(It.IsAny<string>(), null))
            .ReturnsAsync(false);

        _mockBrandRepository.Setup(x => x.Add(It.IsAny<Brand>()))
            .Callback<Brand>(brand => capturedBrand = brand);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        capturedBrand.Should().NotBeNull();

        // This assumes the default value of EntityStatus is the first enum value
        // Adjust this assertion based on your actual enum definition if needed
        capturedBrand.Status.Should().Be(default(EntityStatus));
    }

    [Fact]
    public async Task Handle_WithValidStatus_ShouldCreateWithCorrectStatus()
    {
        // Arrange
        var command = new CreateBrandCommand
        {
            Name = "Sony",
            Slug = "sony",
            Description = "Sony headphones brand",
            Status = "Active"
        };

        Brand capturedBrand = null;

        _mockBrandRepository.Setup(x => x.FindByCondition(
            It.IsAny<Expression<Func<Brand, bool>>>(),
            It.IsAny<CancellationToken>()))
            .Returns(new List<Brand>().AsQueryable());

        _mockBrandRepository.Setup(x => x.IsSlugAlreadyExisted(It.IsAny<string>(), null))
            .ReturnsAsync(false);

        _mockBrandRepository.Setup(x => x.Add(It.IsAny<Brand>()))
            .Callback<Brand>(brand => capturedBrand = brand);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        capturedBrand.Should().NotBeNull();
        capturedBrand.Status.Should().Be(EntityStatus.Active);
    }
}
