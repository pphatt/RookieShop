using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;
using HeadphoneStore.Shared.Dtos.Category;
using HeadphoneStore.Shared.Dtos.Product;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetProductBySlug;

using Exceptions = Domain.Exceptions.Exceptions;

public class GetProductBySlugQueryHandler : IQueryHandler<GetProductBySlugQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductBySlugQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductDto>> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetQueryableSet()
            .AsNoTracking()
            .Where(x => x.Slug == request.Slug)
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .Include(x => x.Media)
            .FirstOrDefaultAsync();

        if (product is null || product.IsDeleted)
            throw new Exceptions.Product.NotFound();

        var result = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            Description = product.Description,
            Stock = product.Stock,
            Sold = product.Sold,
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
            Status = product.Status.ToString(),
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
