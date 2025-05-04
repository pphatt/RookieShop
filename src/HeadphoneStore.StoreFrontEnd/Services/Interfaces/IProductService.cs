using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.StoreFrontEnd.Services.Interfaces;

public interface IProductService
{
    Task<Result<PagedResult<ProductDto>>> GetAllProducts(int pageIndex = 1,
                                                         int pageSize = 10,
                                                         string? searchTerm = null,
                                                         List<Guid>? categoryIds = null);
}
