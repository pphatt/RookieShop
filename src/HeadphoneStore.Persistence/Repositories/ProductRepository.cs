using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Product;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Persistence.Repository;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Persistence.Repositories;

public class ProductRepository : RepositoryBase<Product, Guid>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<ProductDto>> GetAllProductPagination(string? keyword, int pageIndex, int pageSize)
    {
        var query = GetQueryableSet()
            .Include(x => x.Category)
            .Include(x => x.Media)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        query = query
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedDateTime);

        var result = query.Select(x => new ProductDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Quantity = x.Quantity,
            Sku = x.Sku,
            CategoryName = x.Category.Name,
            BrandName = x.Brand.Name,
            ProductStatus = x.ProductStatus.ToString(),
            ProductPrice = x.ProductPrice.Amount,
            AverageRating = x.AverageRating,
            TotalReviews = x.TotalReviews,
            Media = x.Media.Select(x => new ProductMediaDto
            {
                ImageUrl = x.ImageUrl,
                Name = x.Name,
                Path = x.Path,
                PublicId = x.PublicId
            }).ToList().AsReadOnly()
        });

        return await PagedResult<ProductDto>.InitializeAsync(result, pageIndex, pageSize);
    }
}
