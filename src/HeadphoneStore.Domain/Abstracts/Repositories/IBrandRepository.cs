using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Brand;
using HeadphoneStore.Domain.Aggregates.Products.Entities;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IBrandRepository : IRepositoryBase<Brand, Guid>
{
    Task<PagedResult<BrandDto>> GetBrandsPagination(string? keyword,
                                                int pageIndex,
                                                int pageSize);
}
