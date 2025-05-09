using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Persistence.Repository;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;
using HeadphoneStore.Shared.Dtos.Category;
using HeadphoneStore.Shared.Dtos.Product;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Persistence.Repositories;

public class ProductRepository : RepositoryBase<Product, Guid>, IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task AddImageAsync(ProductMedia media)
        => await _context.ProductMedias.AddAsync(media);

    public async Task<bool> IsSlugAlreadyExisted(string slug, Guid? productId = null)
    {
        if (productId is not null)
        {
            return await _context.Products.AnyAsync(x => x.Slug == slug && x.Id != productId);
        }

        return await _context.Products.AnyAsync(x => x.Slug == slug);
    }

    public async Task<PagedResult<ProductDto>> GetAllProductPagination(List<Guid>? categoryIds, string? keyword, int pageIndex, int pageSize)
    {
        var query = GetQueryableSet()
            .Include(x => x.Category)
            .Include(x => x.Media)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => 
                x.Name.ToLowerInvariant().Contains(keyword.ToLowerInvariant()) ||
                x.Description.ToLowerInvariant().Contains(keyword.ToLowerInvariant()));
        }

        if (categoryIds != null && categoryIds.Any())
        {
            query = query.Where(x => categoryIds.Contains(x.CategoryId));
        }

        query = query
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedDateTime);

        var result = query.Select(x => new ProductDto
        {
            Id = x.Id,
            Name = x.Name,
            Slug = x.Slug,
            Description = x.Description,
            Stock = x.Stock,
            Sold = x.Sold,
            Sku = x.Sku,
            Category = new CategoryDto
            {
                Id = x.Category.Id,
                Name = x.Category.Name
            },
            Brand = new BrandDto
            {
                Id = x.Brand.Id,
                Name = x.Brand.Name,
                Slug = x.Brand.Slug
            },
            ProductStatus = x.ProductStatus.ToString(),
            ProductPrice = x.ProductPrice.Amount,
            AverageRating = x.AverageRating,
            TotalReviews = x.TotalReviews,
            Status = x.Status.ToString(),
            Media = x.Media.OrderBy(x => x.DisplayOrder).Select(x => new ProductMediaDto
            {
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                Name = x.Name,
                Path = x.Path,
                PublicId = x.PublicId,
                DIsplayOrder = x.DisplayOrder
            }).ToList().AsReadOnly()
        });

        return await PagedResult<ProductDto>.InitializeAsync(result, pageIndex, pageSize);
    }
}
