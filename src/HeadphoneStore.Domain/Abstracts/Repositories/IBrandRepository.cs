using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IBrandRepository : IRepositoryBase<Brand, Guid>
{
    Task<PagedResult<BrandDto>> GetBrandsPagination(string? keyword,
                                                int pageIndex,
                                                int pageSize);
}
