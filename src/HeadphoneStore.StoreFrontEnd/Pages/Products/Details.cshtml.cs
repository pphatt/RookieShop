using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.StoreFrontEnd.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeadphoneStore.StoreFrontEnd.Pages.Products;

public class DetailsModel : PageModel
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IProductService productService, ICartService cartService, ILogger<DetailsModel> logger)
    {
        _productService = productService;
        _cartService = cartService;
        _logger = logger;
    }

    public ProductDto? Product { get; set; }
    
    public async Task OnGet(string productSlug)
    {
        var product = await _productService.GetProductBySlug(productSlug);

        if (product is not null)
        {
            Product = product;
        }
    }
    
    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId, string productSlug, int quantity = 1)
    {
        await _cartService.AddToCartAsync(productId, quantity);
        return RedirectToPage("/Products/Details", new { productSlug });
    }
}
