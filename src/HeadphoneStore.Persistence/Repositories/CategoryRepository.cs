using Azure.Core;

using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Categories.Entities;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Persistence.Repository;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Persistence.Repositories;

public class CategoryRepository : RepositoryBase<Category, Guid>, ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsSlugAlreadyExisted(string slug, Guid? categoryId = null)
    {
        if (categoryId is not null)
        {
            return await _context.Categories.AnyAsync(x => x.Slug == slug && x.Id != categoryId);
        }

        return await _context.Categories.AnyAsync(x => x.Slug == slug);
    }

    public async Task<Category?> GetCategoryById(Guid categoryId)
    {
        return await GetQueryableSet()
            .AsNoTracking()
            .Where(x => x.Id.Equals(categoryId))
            .Include(x => x.Parent)
            .Include(x => x.SubCategories)
            .SingleOrDefaultAsync();
    }

    public async Task<List<Category>> GetAllFirstLevelCategories(string? searchTerm)
    {
        var query = GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .IgnoreQueryFilters();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x => x.Name.Contains(searchTerm));
        }

        query = query
            .Where(x => x.ParentId == null && x.Status == EntityStatus.Active);

        var result = await query.ToListAsync();

        return result;
    }

    public async Task<List<Category>> GetAllCategoriesWithSubCategories(string categorySlug)
    {
        return await GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .IgnoreQueryFilters()
            .Where(x => x.Status == EntityStatus.Active && x.Slug.Contains(categorySlug))
            .ToListAsync();
    }

    public async Task<List<Category>> GetAllSubCategories()
    {
        return await GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .IgnoreQueryFilters()
            .Where(x => x.Status == EntityStatus.Active)
            .Include(x => x.SubCategories)
            .SelectMany(c => c.SubCategories)
            .Where(x => x.Status == EntityStatus.Active)
            .ToListAsync();
    }

    public async Task<List<Category>> GetAllFirstLevelCategories()
    {
        return await GetQueryableSet()
            .Where(x => x.ParentId == null && 
                        !x.IsDeleted && 
                        x.Status == EntityStatus.Active)
            .ToListAsync();
    }

    public async Task<PagedResult<Category>> GetAllCategoriesPagination(string? searchTerm, int pageIndex, int pageSize)
    {
        var query = GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .IgnoreQueryFilters();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x => x.Name.Contains(searchTerm));
        }

        query = query
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedDateTime);

        return await PagedResult<Category>.InitializeAsync(query, pageIndex, pageSize);
    }
}
