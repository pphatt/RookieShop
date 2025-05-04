using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;
using HeadphoneStore.StoreFrontEnd.Apis.Endpoints;
using HeadphoneStore.StoreFrontEnd.Apis.Interfaces;
using HeadphoneStore.StoreFrontEnd.Services.Interfaces;

namespace HeadphoneStore.StoreFrontEnd.Services;

public class BrandService : IBrandService
{
    private readonly IApiInstance _apiInstance;

    public BrandService(IApiInstance apiInstance)
    {
        _apiInstance = apiInstance;
    }

    public async Task<List<BrandDto>> GetAllBrands(List<Guid>? categoryIds)
    {
        // Build query params
        var queryParams = new List<string>();

        if (categoryIds != null && categoryIds.Any())
        {
            foreach (var id in categoryIds)
            {
                queryParams.Add($"categoryIds={id}");
            }
        }

        string endpoint = $"{BrandApi.GetAllBrandsFilteredByProductProperties}?{string.Join("&", queryParams)}";
        
        var result = await _apiInstance.GetAsync<Result<List<BrandDto>>>(endpoint);

        return result!.Value;
    }
}
