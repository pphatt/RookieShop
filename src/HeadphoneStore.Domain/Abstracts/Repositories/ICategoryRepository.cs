using HeadphoneStore.Domain.Aggregates.Categories.Entities;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category, Guid>
{
    Task<bool> IsSlugAlreadyExisted(string slug, Guid? categoryId = null);

    Task<List<CategoryDto>> GetAllFirstLevelCategories(string? searchTerm);

    Task<PagedResult<CategoryDto>> GetAllCategoriesPagination(string? searchTerm,
                                                              int pageIndex,
                                                              int pageSize);
}
