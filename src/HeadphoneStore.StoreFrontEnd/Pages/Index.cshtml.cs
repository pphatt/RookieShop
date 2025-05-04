using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.StoreFrontEnd.Services.Interfaces;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeadphoneStore.StoreFrontEnd.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IProductService _productService;

    public IndexModel(ILogger<IndexModel> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    /// <summary>
    /// "Tai Nghe" category
    /// </summary>
    private readonly string _headphoneCategorySlug = "tai-nghe";

    /// <summary>
    /// "Dac" category
    /// </summary>
    private readonly string _dacCategorySlug = "dac";

    public List<ProductDto> Headphones { get; set; } = [];

    public List<ProductDto> Dacs { get; set; } = [];

    public async Task OnGet()
    {
        try
        {
            var headphone = await _productService.GetAllProducts(
                categorySlug: _headphoneCategorySlug
            );

            var dac = await _productService.GetAllProducts(
                categorySlug: _dacCategorySlug
            );

            if (headphone?.Value is not null)
            {
                Headphones = headphone.Value.Items;
            }
            
            if (dac?.Value is not null)
            {
                Dacs = dac.Value.Items;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "Exception occurred while fetching products.");
            throw;
        }
    }
}