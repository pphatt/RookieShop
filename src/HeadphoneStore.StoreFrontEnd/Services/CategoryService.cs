using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;
using HeadphoneStore.StoreFrontEnd.Apis.Endpoints;
using HeadphoneStore.StoreFrontEnd.Interfaces.Apis;
using HeadphoneStore.StoreFrontEnd.Interfaces.Services;

namespace HeadphoneStore.StoreFrontEnd.Services;

public class CategoryService : ICategoryService
{
    private readonly IApiInstance _apiInstance;

    private readonly ILogger<CategoryService> _logger;
    
    public CategoryService(IApiInstance apiInstance, ILogger<CategoryService> logger)
    {
        _apiInstance = apiInstance;
        _logger = logger;
    }

    public async Task<List<CategoryDto>> GetAllCategories(string? searchTerm = null)
    {
        // Build query params
        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            queryParams.Add($"searchTerm={Uri.EscapeDataString(searchTerm)}");
        }

        string endpoint = $"{CategoryApi.GetAllCategories}?{string.Join("&", queryParams)}";

        var result = await _apiInstance.GetAsync<Result<List<CategoryDto>>>(endpoint);

        return result!.Value;
    }

    public async Task<List<CategoryDto>> GetAllCategoriesWithSub(string categorySlug)
    {
        // Build query params
        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            queryParams.Add($"categorySlug={Uri.EscapeDataString(categorySlug)}");
        }

        string endpoint = $"{CategoryApi.GetAllCategoriesWithSub}?{string.Join("&", queryParams)}";

        var result = await _apiInstance.GetAsync<Result<List<CategoryDto>>>(endpoint);

        return result!.Value;
    }
}
