using HeadphoneStore.Domain.Aggregates.Brands.Entities;
using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IBrandRepository : IRepositoryBase<Brand, Guid>
{
    Task<bool> IsSlugAlreadyExisted(string slug, Guid? brandId = null);

    Task<PagedResult<Brand>> GetBrandsPagination(string? keyword,
                                                    int pageIndex,
                                                    int pageSize);

    Task<List<Brand>> GetAllBrands();

    Task<List<Brand>> GetBrandsFilteredByProductProperties(List<Guid>? categoryIds);
}
