using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Brand;
using HeadphoneStore.Contract.Dtos.Category;
using HeadphoneStore.Contract.Dtos.Product;
using HeadphoneStore.Domain.Abstracts.Repositories;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetProductById;

using Exceptions = Domain.Exceptions.Exceptions;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetQueryableSet()
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .FirstOrDefaultAsync();

        if (product is null || product.IsDeleted)
            throw new Exceptions.Product.NotFound();

        var result = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Quantity = product.Quantity,
            Sku = product.Sku,
            Category = new CategoryDto
            {
                Id = product.Category.Id,
                Name = product.Category.Name
            },
            Brand = new BrandDto
            {
                Id = product.Brand.Id,
                Name = product.Brand.Name
            },
            ProductStatus = product.ProductStatus.ToString().FormatPascalCaseString(),
            ProductPrice = product.ProductPrice.Amount,
            AverageRating = product.AverageRating,
            TotalReviews = product.TotalReviews,
            Media = product.Media.OrderBy(x => x.Order).Select(x => new ProductMediaDto
            {
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                Name = x.Name,
                Path = x.Path,
                PublicId = x.PublicId,
                Order = x.Order
            }).ToList().AsReadOnly()
        };

        return Result.Success(result);
    }
}
