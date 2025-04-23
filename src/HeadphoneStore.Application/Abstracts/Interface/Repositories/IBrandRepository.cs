using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Brand;
using HeadphoneStore.Domain.Aggregates.Products.Entities;

namespace HeadphoneStore.Application.Abstracts.Interface.Repositories;

public interface IBrandRepository : IRepositoryBase<Brand, Guid>
{
    Task<PagedResult<BrandDto>> GetBrandsPaging(string? keyword,
                                                int pageIndex,
                                                int pageSize);
}
