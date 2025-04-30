using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Categories.Entities;
using HeadphoneStore.Persistence.Repository;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Persistence.Repositories;

public class CategoryRepository : RepositoryBase<Category, Guid>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
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
            Description = x.Description,
            CreatedBy = x.CreatedBy,
            UpdatedBy = x.UpdatedBy,
            Parent = x.Parent != null ? new CategoryDto
            {
                Id = x.Parent.Id,
                Name = x.Parent.Name,
                Description = x.Parent.Description,
                CreatedBy = x.Parent.CreatedBy,
                UpdatedBy = x.Parent.UpdatedBy,
            } : null,
        });

        return await PagedResult<CategoryDto>.InitializeAsync(result, pageIndex, pageSize);
    }
}
