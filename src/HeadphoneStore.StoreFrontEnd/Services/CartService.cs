using HeadphoneStore.StoreFrontEnd.Interfaces.Services;
using HeadphoneStore.StoreFrontEnd.Models.Cart;

using Newtonsoft.Json;

namespace HeadphoneStore.StoreFrontEnd.Services;

public class CartService : ICartService
{
    private readonly IProductService _productService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CartService> _logger;

    private const string CartSessionKey = nameof(CartSessionKey);

    public CartService(IProductService productService, 
                       IHttpContextAccessor httpContextAccessor,
                       ILogger<CartService> logger)
    {
        _productService = productService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<CartViewModel> GetCartAsync()
    {
        var cart = GetCartFromSession();
        
        await RefreshProductDetails(cart);
        
        return cart;
    }

    public async Task<CartViewModel> AddToCartAsync(Guid productId, int quantity)
    {
        var cart = GetCartFromSession();
        
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var product = await _productService.GetProductById(productId);
            
            if (product != null)
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Name = product.Name,
                    Price = product.ProductPrice,
                    ImageUrl = product.Media.FirstOrDefault()?.ImageUrl ?? string.Empty,
                });
            }
        }
        
        SaveCartToSession(cart);
        return cart;
    }

    public async Task<CartViewModel> UpdateCartItemAsync(Guid productId, int quantity)
    {
        var cart = GetCartFromSession();
        
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        
        if (existingItem != null)
        {
            if (quantity <= 0)
            {
                cart.Items.Remove(existingItem);
            }
            else
            {
                existingItem.Quantity = quantity;
            }
            
            SaveCartToSession(cart);
        }
        
        return cart;
    }

    public async Task<CartViewModel> RemoveCartItemAsync(Guid productId)
    {
        var cart = GetCartFromSession();
        
        var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        
        if (itemToRemove != null)
        {
            cart.Items.Remove(itemToRemove);
            SaveCartToSession(cart);
        }
        
        return cart;
    }

    public async Task ClearCartFromSessionAsync()
    {
        _httpContextAccessor.HttpContext?.Session.Remove(CartSessionKey);
    }

    private CartViewModel GetCartFromSession()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var cartJson = session?.GetString(CartSessionKey);
        
        if (string.IsNullOrEmpty(cartJson))
        {
            return new CartViewModel();
        }
        
        return JsonConvert.DeserializeObject<CartViewModel>(cartJson) ?? new CartViewModel();
    }

    private void SaveCartToSession(CartViewModel cart)
    {
        cart.TotalCartItems = cart.Items.Count;
        cart.TotalCartPrice = cart.Items.Sum(i => i.Price * i.Quantity);
        
        // Save to session
        var cartJson = JsonConvert.SerializeObject(cart);
        _httpContextAccessor.HttpContext?.Session.SetString(CartSessionKey, cartJson);
    }

    private async Task RefreshProductDetails(CartViewModel cart)
    {
        bool cartChanged = false;
        
        foreach (var item in cart.Items.ToList())
        {
            var product = await _productService.GetProductById(item.ProductId);
            
            if (product != null)
            {
                // Update with current price and details
                item.Name = product.Name;
                item.Price = product.ProductPrice;
                item.ImageUrl = product.Media.FirstOrDefault()?.ImageUrl ?? string.Empty;
                item.Category = product.Category;
                item.Brand = product.Brand;
                
                // Check if the requested quantity is still available
                if (item.Quantity > product.Stock)
                {
                    item.Quantity = (int)product.Stock;
                    cartChanged = true;
                }
            }
            else
            {
                // Remove item if product no longer exists
                cart.Items.Remove(item);
                cartChanged = true;
            }
        }
        
        // Only save if the cart was modified
        if (cartChanged)
        {
            SaveCartToSession(cart);
        }
        else
        {
            // Still recalculate in case prices have changed
            cart.TotalCartItems = cart.Items.Count;
            cart.TotalCartPrice = cart.Items.Sum(i => i.Price * i.Quantity);
        }
    }
}
