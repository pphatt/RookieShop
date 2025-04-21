using HeadphoneStore.Application.Abstracts.Interface.Services.Media;
using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;

using Exceptions = Domain.Exceptions.Exceptions;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICloudinaryService _cloudinaryService;

    public CreateProductCommandHandler(
        UserManager<AppUser> userManager,
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        ICloudinaryService cloudinaryService)
    {
        _userManager = userManager;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _productRepository.FindByCondition(x => x.Name == request.Name);

        if (product is not null)
        {
            throw new Exceptions.Product.DuplicateName();
        }

        if (request.ProductPrice < 0)
        {
            throw new Exceptions.Product.InvalidPrice();
        }

        var category = _categoryRepository.FindByIdAsync(request.CategoryId);

        if (category is null)
        {
            throw new Exceptions.Category.NotFound();
        }

        var brand = _brandRepository.FindByIdAsync(request.BrandId);

        if (brand is null)
        {
            throw new Exceptions.Brand.NotFound();
        }

        return Result.Success();
    }
}
