using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Category;
using HeadphoneStore.Domain.Aggregates.Categories.Entities;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category, Guid>
{
    Task<PagedResult<CategoryDto>> GetAllCategoriesPagination(string? keyword,
                                                              int pageIndex,
                                                              int pageSize);
}
