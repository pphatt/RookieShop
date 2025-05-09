using HeadphoneStore.Shared.Services.Product.CreateProductRating;

namespace HeadphoneStore.StoreFrontEnd.Interfaces.Services;

public interface IProductRatingService
{
    Task CreateProductRating(CreateProductRatingRequestDto model);
}
