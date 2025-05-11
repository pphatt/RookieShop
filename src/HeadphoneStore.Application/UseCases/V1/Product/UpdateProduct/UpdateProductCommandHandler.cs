using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.Abstracts.Interface.Services.Media;
using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Enumerations;
using HeadphoneStore.Domain.Aggregates.Products.ValueObjects;
using HeadphoneStore.Domain.Constraints;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Media;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;

using Exceptions = Domain.Exceptions.Exceptions;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        ICloudinaryService cloudinaryService,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _cloudinaryService = cloudinaryService;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productFromDb = await _productRepository
            .GetQueryableSet()
            .Where(x => x.Id == request.Id)
            .Include(x => x.Media)
            .FirstOrDefaultAsync();

        if (productFromDb is null)
            throw new Exceptions.Product.NotFound();

        if (productFromDb.IsDeleted)
            throw new Exceptions.Product.AlreadyDeleted();

        var duplicateName = _productRepository
            .FindByCondition(x => x.Name == request.Name)
            .FirstOrDefault();

        if (duplicateName is not null && duplicateName.Id != productFromDb.Id)
            throw new Exceptions.Product.DuplicateName();

        var slug = productFromDb.Name != request.Name ? request.Name.Slugify() : request.Slug;
        var isSlugAlreadyExisted = await _categoryRepository.IsSlugAlreadyExisted(slug, productFromDb.Id);

        if (isSlugAlreadyExisted)
            throw new Exceptions.Product.SlugExists();

        var category = await _categoryRepository.FindByIdAsync(request.CategoryId);

        if (category is null)
            throw new Exceptions.Category.NotFound();

        var brand = await _brandRepository.FindByIdAsync(request.BrandId);

        if (brand is null)
            throw new Exceptions.Brand.NotFound();

        // handle remove old images if changes.
        if (request.OldImages is not null &&
            request.OldImages.Count() > 0 &&
            productFromDb.Media.Count() > 0)
        {
            var oldImages = new List<DeleteFileDto>();

            var oldImagesRequest = request.OldImages;

            foreach (var image in productFromDb.Media.ToList())
            {
                if (oldImagesRequest.Contains(image.Id))
                {
                    continue;
                }

                oldImages.Add(new DeleteFileDto
                {
                    Type = FileType.Image,
                    PublicId = image.PublicId
                });

                productFromDb.RemoveMedia(image);
            }

            if (oldImages.Count() > 0)
            {
                await _cloudinaryService.RemoveFilesFromCloudinary(oldImages);
            }

            var oldImagesOrder = request.ListOrder
                .Where(x => x.Split("-")[0] == "old")
                .Select(x => x.Split("-")[1])
                .ToList();

            foreach (var imageInfo in request.OldImages.Select((value, i) => (value, i)))
            {
                var image = productFromDb.Media.FirstOrDefault(x => x.Id == imageInfo.value);

                image!.DisplayOrder = Int32.Parse(oldImagesOrder[imageInfo.i]);
            }
        }

        // handle add new images if added.
        if (request.NewImages is not null && request.NewImages.Count() > 0)
        {
            var required = new FileRequiredParamsDto
            {
                type = FileType.Image,
                userId = request.UpdatedBy,
                productId = productFromDb.Id,
            };

            var filesInfo = await _cloudinaryService.UploadFilesToCloudinary(request.NewImages, required);
            var newImagesOrder = request.ListOrder
                .Where(x => x.Split("-")[0] == "new")
                .Select(x => x.Split("-")[1])
                .ToList();

            foreach (var info in filesInfo.Select((value, i) => (value, i)))
            {
                var file = ProductMedia.Create(
                    productId: productFromDb.Id,
                    imageUrl: info.value.Path,
                    publicId: info.value.PublicId,
                    path: info.value.Path,
                    name: info.value.Name,
                    displayOrder: Int32.Parse(newImagesOrder[info.i]));

                await _productRepository.AddImageAsync(file);
            }
        }

        Enum.TryParse<ProductStatus>(request.ProductStatus, false, out var status);
        Enum.TryParse<EntityStatus>(request.Status, false, out var entityStatus);

        productFromDb.Update(
            name: request.Name,
            description: request.Description,
            stock: request.Stock,
            productStatus: status,
            productPrice: new ProductPrice(request.ProductPrice),
            sku: slug,
            slug: slug,
            sold: 0,
            category: category,
            brand: brand,
            status: entityStatus
        );

        await _productRepository.UpdateAsync(productFromDb, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("Products", cancellationToken);

        return Result.Success("Update product successfully.");
    }
}
