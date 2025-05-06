using HeadphoneStore.StoreFrontEnd.Models.Cart;

namespace HeadphoneStore.StoreFrontEnd.Interfaces.Services;

public interface ICartService
{
    Task<CartViewModel> GetCartAsync();
    Task<CartViewModel> AddToCartAsync(Guid productId, int quantity);
    Task<CartViewModel> UpdateCartItemAsync(Guid productId, int quantity);
    Task<CartViewModel> RemoveCartItemAsync(Guid productId);
    Task ClearCartFromSessionAsync();
}