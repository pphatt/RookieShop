using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.StoreFrontEnd.Apis.Endpoints;
using HeadphoneStore.StoreFrontEnd.Interfaces.Apis;
using HeadphoneStore.StoreFrontEnd.Interfaces.Services;

namespace HeadphoneStore.StoreFrontEnd.Services;

public class ProductService : IProductService
{
    private readonly IApiInstance _apiInstance;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IApiInstance apiInstance, ILogger<ProductService> logger)
    {
        _apiInstance = apiInstance;
        _logger = logger;
    }

    public async Task<PagedResult<ProductDto>> GetAllProducts(int pageIndex = 1,
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
        
        if (result is null)
            return null!;

        return result.Value;
    }

    public async Task<ProductDto> GetProductById(Guid productId)
    {
        string endpoint = $"{ProductApi.GetProduct}/{productId}";
        
        var result = await _apiInstance.GetAsync<Result<ProductDto>>(endpoint);
        
        if (result is null)
            return null!;

        return result.Value;
    }

    public async Task<ProductDto> GetProductBySlug(string slug)
    {
        string endpoint = $"{ProductApi.GetProduct}/slug/{slug}";
        
        var result = await _apiInstance.GetAsync<Result<ProductDto>>(endpoint);

        if (result is null)
            return null!;

        return result.Value;
    }
}
