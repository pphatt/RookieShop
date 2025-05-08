using HeadphoneStore.Shared.Services.ProductRating.CreateProductRating;

namespace HeadphoneStore.StoreFrontEnd.Interfaces.Services;

public interface IProductRatingService
{
    Task CreateProductRating(CreateProductRatingRequestDto model);
}
