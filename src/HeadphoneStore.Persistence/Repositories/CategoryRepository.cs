using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Categories.Entities;
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

    public async Task<PagedResult<CategoryDto>> GetAllCategoriesPagination(string? keyword, int pageIndex, int pageSize)
    {
        var query = GetQueryableSet();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
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
            CreatedBy = x.CreatedBy,
            UpdatedBy = x.UpdatedBy,
            Parent = x.Parent != null ? new CategoryDto
            {
                Id = x.Parent.Id,
                Name = x.Parent.Name,
                Slug = x.Slug,
                Description = x.Parent.Description,
                Status = x.Parent.Status.ToString(),
                CreatedBy = x.Parent.CreatedBy,
                UpdatedBy = x.Parent.UpdatedBy,
            } : null,
        });

        return await PagedResult<CategoryDto>.InitializeAsync(result, pageIndex, pageSize);
    }
}
