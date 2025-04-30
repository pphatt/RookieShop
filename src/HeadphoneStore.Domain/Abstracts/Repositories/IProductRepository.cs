using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IProductRepository : IRepositoryBase<Product, Guid>
{
    Task<PagedResult<ProductDto>> GetAllProductPagination(string? keyword,
                                                          int pageIndex,
                                                          int pageSize);
}
