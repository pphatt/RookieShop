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

    public async Task<List<CategoryDto>> GetAllCategories()
    {
        var result = await _apiInstance.GetAsync<Result<List<CategoryDto>>>(CategoryApi.GetAllCategories);

        return result!.Value;
    }
}
