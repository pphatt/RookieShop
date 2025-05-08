using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Services.ProductRating.CreateProductRating;
using HeadphoneStore.StoreFrontEnd.Apis.Endpoints;
using HeadphoneStore.StoreFrontEnd.Interfaces.Apis;
using HeadphoneStore.StoreFrontEnd.Interfaces.Services;

namespace HeadphoneStore.StoreFrontEnd.Services;

public class ProductRatingService : IProductRatingService
{
    private readonly IApiInstance _apiInstance;

    private readonly ILogger<ProductRatingService> _logger;
    
    public ProductRatingService(IApiInstance apiInstance, ILogger<ProductRatingService> logger)
    {
        _apiInstance = apiInstance;
        _logger = logger;
    }

    public async Task CreateProductRating(CreateProductRatingRequestDto model)
    {
        await _apiInstance.PostAsync<CreateProductRatingRequestDto, Result<string>>(ProductRatingApi.CreateProductRating, model);
    }
}