using HeadphoneStore.Application.Abstracts.Interface.Services.Media;
using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Media;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Aggregates.Products.ValueObjects;
using HeadphoneStore.Domain.Constraints;

using Microsoft.AspNetCore.Identity;

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

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        ICloudinaryService cloudinaryService,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _cloudinaryService = cloudinaryService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productFromDb = _productRepository.FindByCondition(x => x.Name == request.Name).SingleOrDefault();

        if (productFromDb is not null)
            throw new Exceptions.Product.DuplicateName();

        if (request.ProductPrice < 0)
            throw new Exceptions.Product.InvalidPrice();

        var category = await _categoryRepository.FindByIdAsync(request.CategoryId);

        if (category is null)
            throw new Exceptions.Category.NotFound();

        var brand = await _brandRepository.FindByIdAsync(request.BrandId);

        if (brand is null)
            throw new Exceptions.Brand.NotFound();

        var productPrice = ProductPrice.Create(request.ProductPrice);

        var sku = request.Sku.ToString();

        var product = new Product(
            name: request.Name,
            description: request.Description,
            productStatus: request.ProductStatus,
            productPrice: productPrice,
            sku: sku,
            category: category,
            brand: brand,
            createdBy: request.CreatedBy
        );

        product.Quantity = request.Quantity;

        if (request.Files != null && request.Files.Any())
        {
            var required = new FileRequiredParamsDto
            {
                type = FileType.Image,
                userId = request.CreatedBy,
                productId = product.Id
            };

            var uploadResult = await _cloudinaryService.UploadFilesToCloudinary(request.Files, required);

            foreach (var info in uploadResult)
            {
                var media = new ProductMedia(
                    productId: product.Id,
                    imageUrl: info.Path,
                    publicId: info.PublicId,
                    path: info.Path,
                    name: info.Name,
                    createdBy: request.CreatedBy);

                product.AddMedia(media);
            }
        }

        _productRepository.Add(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
