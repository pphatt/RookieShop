using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.StoreFrontEnd.Interfaces.Services;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetAllProducts(int pageIndex = 1,
                                                         int pageSize = 10,
                                                         string? searchTerm = null,
                                                         List<Guid>? categoryIds = null);

    Task<ProductDto> GetProductById(Guid productId);
    
    Task<ProductDto> GetProductBySlug(string slug);
}
