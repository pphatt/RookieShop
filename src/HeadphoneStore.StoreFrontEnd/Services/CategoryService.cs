using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;
using HeadphoneStore.StoreFrontEnd.Apis.Endpoints;
using HeadphoneStore.StoreFrontEnd.Apis.Interfaces;
using HeadphoneStore.StoreFrontEnd.Services.Interfaces;

namespace HeadphoneStore.StoreFrontEnd.Services;

public class CategoryService : ICategoryService
{
    private readonly IApiInstance _apiInstance;

    public CategoryService(IApiInstance apiInstance)
    {
        _apiInstance = apiInstance;
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
}
