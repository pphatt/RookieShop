using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Application.Abstracts.Interface.Services.Media;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Enumerations;
using HeadphoneStore.Domain.Aggregates.Products.ValueObjects;
using HeadphoneStore.Domain.Constraints;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Media;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;

using Exceptions = Domain.Exceptions.Exceptions;
using Product = Domain.Aggregates.Products.Entities.Product;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public CreateProductCommandHandler(
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

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var duplicateName = _productRepository.FindByCondition(x => x.Name == request.Name).SingleOrDefault();

        if (duplicateName is not null)
            throw new Exceptions.Product.DuplicateName();

        if (request.ProductPrice < 0)
            throw new Exceptions.Product.InvalidPrice();

        var category = await _categoryRepository.FindByIdAsync(request.CategoryId);

        if (category is null)
            throw new Exceptions.Category.NotFound();

        var brand = await _brandRepository.FindByIdAsync(request.BrandId);

        if (brand is null)
            throw new Exceptions.Brand.NotFound();

        var isSlugAlreadyExisted = await _productRepository.IsSlugAlreadyExisted(request.Slug!);

        if (isSlugAlreadyExisted)
            throw new Exceptions.Product.SlugExists();

        var productPrice = ProductPrice.Create(request.ProductPrice);

        var sku = request.Sku.ToString();

        Enum.TryParse<ProductStatus>(request.ProductStatus, true, out var productStatus);
        Enum.TryParse<EntityStatus>(request.Status, true, out var status);

        var product = Product.Create(
            name: request.Name,
            description: request.Description,
            productStatus: productStatus,
            productPrice: productPrice,
            sku: sku,
            slug: request.Slug!,
            stock: request.Stock,
            sold: 0,
            category: category,
            brand: brand,
            status: status
        );

        if (request.Images != null && request.Images.Any())
        {
            var required = new FileRequiredParamsDto
            {
                type = FileType.Image,
                userId = request.CreatedBy,
                productId = product.Id
            };

            var uploadResult = await _cloudinaryService.UploadFilesToCloudinary(request.Images, required);

            foreach (var info in uploadResult.Select((value, i) => (value, i)))
            {
                var media = ProductMedia.Create(
                    productId: product.Id,
                    imageUrl: info.value.Path,
                    publicId: info.value.PublicId,
                    path: info.value.Path,
                    name: info.value.Name,
                    displayOrder: info.i + 1);

                product.AddMedia(media);
            }
        }

        _productRepository.Add(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("Products", cancellationToken);

        return Result.Success("Create new product successfully.");
    }
}
