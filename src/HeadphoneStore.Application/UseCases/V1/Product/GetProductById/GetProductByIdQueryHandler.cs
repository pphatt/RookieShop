using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;
using HeadphoneStore.Shared.Dtos.Category;
using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.Shared.Dtos.ProductRating;

using Microsoft.AspNetCore.Identity;
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
            .Include(x => x.Media)
            .Include(x => x.Ratings)
                .ThenInclude(x => x.Customer)
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
            }).ToList().AsReadOnly(),
            Rating = product.Ratings.OrderBy(x => x.CreatedDateTime).Select(x => new ProductRatingDto
            {
                CustomerAvatar = x.Customer.Avatar,
                CustomerName = x.Customer.UserName,
                RatingValue = x.RatingValue.ToString(),
                Comment = x.Comment,
                CreatedAt = x.CreatedDateTime,
                UpdatedAt = x.UpdatedDateTime
            }).ToList().AsReadOnly()
        };

        return Result.Success(result);
    }
}
