using HeadphoneStore.Domain.Aggregates.Categories.Entities;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category, Guid>
{
    Task<bool> IsSlugAlreadyExisted(string slug, Guid? categoryId = null);

    Task<Category> GetCategoryById(Guid categoryId);

    Task<List<Category?>> GetAllFirstLevelCategories(string? searchTerm);

    Task<List<Category>> GetAllCategoriesWithSubCategories(string categorySlug);

    Task<List<Category>> GetAllSubCategories();

    Task<List<Category>> GetAllFirstLevelCategories();

    Task<PagedResult<Category>> GetAllCategoriesPagination(string? searchTerm,
                                                              int pageIndex,
                                                              int pageSize);
}
