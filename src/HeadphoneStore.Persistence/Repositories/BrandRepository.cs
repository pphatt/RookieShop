using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Brands.Entities;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Persistence.Repository;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Persistence.Repositories;

public class BrandRepository : RepositoryBase<Brand, Guid>, IBrandRepository
{
    private readonly ApplicationDbContext _context;

    public BrandRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsSlugAlreadyExisted(string slug, Guid? brandId = null)
    {
        if (brandId is not null)
        {
            return await _context.Brands.AnyAsync(x => x.Slug == slug && x.Id != brandId);
        }

        return await _context.Brands.AnyAsync(x => x.Slug == slug);
    }

    public async Task<PagedResult<Brand>> GetBrandsPagination(string? keyword, int pageIndex, int pageSize)
    {
        var query = GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .IgnoreQueryFilters();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        query = query
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedDateTime);

        return await PagedResult<Brand>.InitializeAsync(query, pageIndex, pageSize);
    }

    public async Task<List<Brand>> GetAllBrands()
    {
        return await GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .IgnoreQueryFilters()
            .Where(x => !x.IsDeleted && x.Status == EntityStatus.Active)
            .ToListAsync();
    }

    public async Task<List<Brand>> GetBrandsFilteredByProductProperties(List<Guid>? categoryIds)
    {
        var query = GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Products)
                .ThenInclude(x => x.Category)
            .IgnoreQueryFilters();

        if (categoryIds != null && categoryIds.Any())
        {
            query = query
                .Where(x => x.Products.Where(x => categoryIds.Contains(x.CategoryId)).Any());
        }

        var result = await query.ToListAsync();

        return result;
    }
}
