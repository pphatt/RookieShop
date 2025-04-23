using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Product;
using HeadphoneStore.Domain.Aggregates.Products.Entities;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IProductRepository : IRepositoryBase<Product, Guid>
{
    Task<PagedResult<ProductDto>> GetAllProductPagination(string? keyword,
                                                          int pageIndex,
                                                          int pageSize);
}
