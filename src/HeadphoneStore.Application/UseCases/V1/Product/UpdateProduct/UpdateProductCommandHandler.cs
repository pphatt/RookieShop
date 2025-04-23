using HeadphoneStore.Application.Abstracts.Interface.Repositories;
using HeadphoneStore.Application.Abstracts.Interface.Services.Media;
using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Media;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Aggregates.Products.ValueObjects;
using HeadphoneStore.Domain.Constraints;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;

using Exceptions = Domain.Exceptions.Exceptions;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IProductMediaRepository _productMediaRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IProductMediaRepository productMediaRepository,
        ICloudinaryService cloudinaryService,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _productMediaRepository = productMediaRepository;
        _cloudinaryService = cloudinaryService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetQueryableSet()
            .Where(x => x.Id == request.Id)
            .Include(x => x.Media)
            .FirstOrDefaultAsync();

        if (product is null)
            throw new Exceptions.Product.NotFound();

        var duplicateName = _productRepository
            .FindByCondition(x => x.Name == request.Name)
            .FirstOrDefault();

        if (duplicateName is not null && duplicateName.Id != product.Id)
            throw new Exceptions.Product.DuplicateName();

        var category = await _categoryRepository.FindByIdAsync(request.CategoryId);

        if (category is null)
            throw new Exceptions.Category.NotFound();

        var brand = await _brandRepository.FindByIdAsync(request.BrandId);

        if (brand is null)
            throw new Exceptions.Brand.NotFound();

        // handle remove old images if changes.
        if (request.OldFiles is not null &&
            request.OldFiles.Count() > 0 &&
            product.Media.Count() > 0)
        {
            var oldImages = new List<DeleteFileDto>();

            var oldImagesRequest = request.OldFiles;

            foreach (var image in product.Media.ToList())
            {
                if (!oldImagesRequest.Contains(image.Id))
                {
                    oldImages.Add(new DeleteFileDto
                    {
                        Type = FileType.Image,
                        PublicId = image.PublicId
                    });

                    _productMediaRepository.Remove(image);
                }
            }

            if (oldImages.Count() > 0)
            {
                await _cloudinaryService.RemoveFilesFromCloudinary(oldImages);
            }
        }

        // handle add new images if added.
        if (request.NewFiles is not null && request.NewFiles.Count() > 0)
        {
            var required = new FileRequiredParamsDto
            {
                type = FileType.Image,
                userId = request.UpdatedBy,
                productId = product.Id,
            };

            var filesInfo = await _cloudinaryService.UploadFilesToCloudinary(request.NewFiles, required);

            foreach (var info in filesInfo)
            {
                var file = new ProductMedia(
                    productId: product.Id,
                    imageUrl: info.Path,
                    publicId: info.PublicId,
                    path: info.Path,
                    name: info.Name,
                    createdBy: request.UpdatedBy);

                _productMediaRepository.Add(file);
            }
        }

        product.Update(
            name: request.Name,
            description: request.Description,
            productPrice: new ProductPrice(request.ProductPrice),
            sku: request.Sku.ToString(),
            category: category,
            brand: brand,
            updatedBy: request.UpdatedBy
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Update product successfully.");
    }
}
