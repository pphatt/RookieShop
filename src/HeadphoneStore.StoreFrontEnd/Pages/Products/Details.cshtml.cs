using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.StoreFrontEnd.Services.Interfaces;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeadphoneStore.StoreFrontEnd.Pages.Products;

public class DetailsModel : PageModel
{
    private readonly ILogger<DetailsModel> _logger;
    private readonly IProductService _productService;

    public DetailsModel(IProductService productService, ILogger<DetailsModel> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    public ProductDto Product { get; set; }
    
    public async Task OnGet(Guid productId)
    {
        var product = await _productService.GetProductById(productId);

        if (product is not null)
        {
            Product = product.Value;
        }
    }
}