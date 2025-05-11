using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IProductRepository : IRepositoryBase<Product, Guid>
{
    Task AddImageAsync(ProductMedia media);

    Task<bool> IsSlugAlreadyExisted(string slug, Guid? productId = null);

    Task<Product?> GetProductBySlug(string slug);

    Task<Product?> GetProductById(Guid id);

    Task<PagedResult<ProductDto>> GetAllProductPagination(List<Guid>? categoryIds, 
                                                          string? keyword,
                                                          int pageIndex,
                                                          int pageSize);
}
