using Azure.Core;

using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Persistence.Repository;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;
using HeadphoneStore.Shared.Dtos.Category;
using HeadphoneStore.Shared.Dtos.Product;

using MediatR;

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

    public async Task<Product?> GetProductBySlug(string slug)
    {
        return await GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .IgnoreQueryFilters()
            .Where(x => x.Slug == slug)
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .Include(x => x.Media)
            .Include(x => x.Ratings)
                .ThenInclude(x => x.Customer)
            .FirstOrDefaultAsync();
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        return await GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .IgnoreQueryFilters()
            .Where(x => x.Id == id)
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .Include(x => x.Media)
            .Include(x => x.Ratings)
                .ThenInclude(x => x.Customer)
            .FirstOrDefaultAsync();
    }

    public async Task<PagedResult<Product>> GetAllProductsPagination(List<Guid>? categoryIds, string? searchTerm, int pageIndex, int pageSize)
    {
        var query = GetQueryableSet()
            .Include(x => x.Category)
            .Include(x => x.Media)
            .AsNoTracking()
            .AsQueryable()
            .IgnoreQueryFilters();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x =>
                x.Name.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()) ||
                x.Description.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()));
        }

        if (categoryIds != null && categoryIds.Any())
        {
            query = query.Where(x => categoryIds.Contains(x.CategoryId));
        }

        query = query
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedDateTime);

        return await PagedResult<Product>.InitializeAsync(query, pageIndex, pageSize);
    }
}
