using HeadphoneStore.Domain.Aggregates.Categories.Entities;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category, Guid>
{
    Task<PagedResult<CategoryDto>> GetAllCategoriesPagination(string? keyword,
                                                              int pageIndex,
                                                              int pageSize);
}
