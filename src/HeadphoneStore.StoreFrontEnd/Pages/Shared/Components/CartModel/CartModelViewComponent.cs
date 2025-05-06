using HeadphoneStore.StoreFrontEnd.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.StoreFrontEnd.Pages.Shared.Components.CartModel;

public class CartModelViewComponent : ViewComponent
{
    private readonly ICartService _cartService;
    
    public CartModelViewComponent(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var cart = await _cartService.GetCartAsync();
        
        return View(cart);
    }
}
