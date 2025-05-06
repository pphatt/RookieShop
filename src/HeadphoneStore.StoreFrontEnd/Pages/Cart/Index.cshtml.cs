using HeadphoneStore.StoreFrontEnd.Interfaces.Services;
using HeadphoneStore.StoreFrontEnd.Models.Cart;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeadphoneStore.StoreFrontEnd.Pages.Cart;

public class IndexModel : PageModel
{
    private readonly ICartService _cartService;

    public CartViewModel Cart { get; set; }
    
    public IndexModel(ICartService cartService)
    {
        _cartService = cartService;
    }
    
    public async Task OnGetAsync()
    {
        Cart = await _cartService.GetCartAsync();
    }
    
    public async Task<IActionResult> OnPostDecreaseQuantityAsync(Guid productId, int quantity)
    {
        await _cartService.UpdateCartItemAsync(productId,  quantity- 1);
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostIncreaseQuantityAsync(Guid productId, int quantity)
    {
        await _cartService.UpdateCartItemAsync(productId, quantity + 1);
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostRemoveCartItemAsync(Guid productId)
    {
        await _cartService.RemoveCartItemAsync(productId);
        
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostClearCartAsync()
    {
        await _cartService.ClearCartFromSessionAsync();
        
        return RedirectToPage();
    }
}
