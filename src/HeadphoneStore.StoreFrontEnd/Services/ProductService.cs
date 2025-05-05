
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.StoreFrontEnd.Apis.Endpoints;
using HeadphoneStore.StoreFrontEnd.Apis.Interfaces;
using HeadphoneStore.StoreFrontEnd.Services.Interfaces;

namespace HeadphoneStore.StoreFrontEnd.Services;

public class ProductService : IProductService
{
    private readonly IApiInstance _apiInstance;

    public ProductService(IApiInstance apiInstance)
    {
        _apiInstance = apiInstance;
    }

    public async Task<Result<PagedResult<ProductDto>>> GetAllProducts(int pageIndex = 1,
                                                                      int pageSize = 10,
                                                                      string? searchTerm = null,
                                                                      List<Guid>? categoryIds = null)
    {
        // Build query params
        var queryParams = new List<string>
        {
            $"pageIndex={pageIndex}",
            $"pageSize={pageSize}"
        };

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            queryParams.Add($"searchTerm={Uri.EscapeDataString(searchTerm)}");
        }

        if (categoryIds != null && categoryIds.Any())
        {
            foreach (var id in categoryIds)
            {
                queryParams.Add($"categoryIds={id}");
            }
        }

        string endpoint = $"{ProductApi.GetAllProducts}?{string.Join("&", queryParams)}";

        var result = await _apiInstance.GetAsync<Result<PagedResult<ProductDto>>>(endpoint);

        return result.Value;
    }

    public async Task<Result<ProductDto>> GetProductById(Guid productId)
    {
        string endpoint = $"{ProductApi.GetProductById}/{productId}";
        
        var result = await _apiInstance.GetAsync<Result<ProductDto>>(endpoint);

        return result.Value;
    }
}
