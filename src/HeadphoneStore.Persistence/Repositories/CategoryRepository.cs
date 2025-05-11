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

    public async Task<List<CategoryDto>> GetAllFirstLevelCategories(string? searchTerm)
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

        var result = await query
            .OrderBy(x => x.CreatedDateTime)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
                SubCategories = x.SubCategories
                    .Where(x => x.Status == EntityStatus.Active)
                    .OrderBy(x => x.CreatedDateTime)
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        Status = c.Status.ToString(),
                    })
            })
            .ToListAsync();

        return result;
    }

    public async Task<List<CategoryDto>> GetAllCategoriesWithSubCategories(string categorySlug)
    {
        return await GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .IgnoreQueryFilters()
            .Where(x => x.Status == EntityStatus.Active && x.Slug.Contains(categorySlug))
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
            })
            .ToListAsync();
    }

    public async Task<List<CategoryDto>> GetAllSubCategories()
    {
        return await GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .IgnoreQueryFilters()
            .Where(x => x.Status == EntityStatus.Active)
            .Include(x => x.SubCategories)
            .SelectMany(c => c.SubCategories)
            .Where(x => x.Status == EntityStatus.Active)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
            })
            .ToListAsync();
    }

    public async Task<List<CategoryDto>> GetAllFirstLevelCategories()
    {
        return await GetQueryableSet()
            .Where(x => x.ParentId == null && 
                        !x.IsDeleted && 
                        x.Status == EntityStatus.Active)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
            })
            .ToListAsync();
    }

    public async Task<PagedResult<CategoryDto>> GetAllCategoriesPagination(string? searchTerm, int pageIndex, int pageSize)
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

        var result = query.Select(x => new CategoryDto
        {
            Id = x.Id,
            Name = x.Name,
            Slug = x.Slug,
            Description = x.Description,
            Status = x.Status.ToString(),
            Parent = x.Parent != null ? new CategoryDto
            {
                Id = x.Parent.Id,
                Name = x.Parent.Name,
                Slug = x.Slug,
                Description = x.Parent.Description,
                Status = x.Parent.Status.ToString(),
            } : null,
        });

        return await PagedResult<CategoryDto>.InitializeAsync(result, pageIndex, pageSize);
    }
}
