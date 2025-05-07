using System.Linq.Expressions;

using FluentAssertions;

using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumerations;

using Microsoft.AspNetCore.Identity;

using Moq;

namespace HeadphoneStore.UnitTests.Application.Brand.Commands.DeleteBrand;

using Brand = Domain.Aggregates.Products.Entities.Brand;
using Exceptions = Domain.Exceptions.Exceptions;

[Trait("Brand", "DeleteBrand")]
public class DeleteBrandCommandHandlerTests : BaseTest
{
    private readonly Mock<UserManager<AppUser>> _mockUserManager;
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly DeleteBrandCommandHandler _handler;

    public DeleteBrandCommandHandlerTests()
    {
        _mockUserManager = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
        _mockBrandRepository = new Mock<IBrandRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCacheService = new Mock<ICacheService>();
        _handler = new DeleteBrandCommandHandler(
            _mockUserManager.Object,
            _mockUnitOfWork.Object,
            _mockBrandRepository.Object,
            _mockCacheService.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldDeleteBrandSuccessfully()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var command = new DeleteBrandCommand
        {
            Id = brandId,
            UpdatedBy = updatedBy
        };

        var user = new AppUser { Id = updatedBy };
        var brand = Brand.Create("TestBrand", "test-slug", "Test description", Guid.NewGuid(), EntityStatus.Active);
        brand.Id = brandId;

        _mockUserManager.Setup(x => x.FindByIdAsync(updatedBy.ToString())).ReturnsAsync(user);
        _mockBrandRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Brand, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(new List<Brand> { brand }.AsQueryable());
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _mockCacheService.Setup(x => x.RemoveByPrefixAsync("Brands", It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Be("Delete brand successfully.");
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ShouldThrowIdentityNotFoundException()
    {
        // Arrange
        var command = new DeleteBrandCommand
        {
            Id = Guid.NewGuid(),
            UpdatedBy = Guid.NewGuid()
        };

        _mockUserManager.Setup(x => x.FindByIdAsync(command.UpdatedBy.ToString())).ReturnsAsync((AppUser)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Identity.NotFound>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonExistentBrand_ShouldThrowBrandNotFoundException()
    {
        // Arrange
        var updatedBy = Guid.NewGuid();
        var command = new DeleteBrandCommand
        {
            Id = Guid.NewGuid(),
            UpdatedBy = updatedBy
        };

        var user = new AppUser { Id = updatedBy };
        _mockUserManager.Setup(x => x.FindByIdAsync(updatedBy.ToString())).ReturnsAsync(user);
        _mockBrandRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Brand, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(new List<Brand>().AsQueryable());

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Brand.NotFound>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithAlreadyDeletedBrand_ShouldThrowAlreadyDeletedException()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var command = new DeleteBrandCommand
        {
            Id = brandId,
            UpdatedBy = updatedBy
        };

        var user = new AppUser { Id = updatedBy };
        var brand = Brand.Create("TestBrand", "test-slug", "Test description", Guid.NewGuid(), EntityStatus.Active);
        brand.Id = brandId;
        brand.IsDeleted = true;

        _mockUserManager.Setup(x => x.FindByIdAsync(updatedBy.ToString())).ReturnsAsync(user);
        _mockBrandRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Brand, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(new List<Brand> { brand }.AsQueryable());

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.Brand.AlreadyDeleted>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}
