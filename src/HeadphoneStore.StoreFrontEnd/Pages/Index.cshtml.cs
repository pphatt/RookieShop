using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.StoreFrontEnd.Interfaces.Services;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeadphoneStore.StoreFrontEnd.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public IndexModel(ILogger<IndexModel> logger, IProductService productService, ICategoryService categoryService)
    {
        _logger = logger;
        _productService = productService;
        _categoryService = categoryService;
    }

    public List<ProductDto> Headphones { get; set; } = [];

    public List<ProductDto> Dacs { get; set; } = [];

    public async Task OnGet()
    {
        try
        {
            var headphoneCategory = await _categoryService.GetAllCategories(searchTerm: "Tai Nghe");
            var dacCategory = await _categoryService.GetAllCategories(searchTerm: "Dac");

            var headphone = await _productService.GetAllProducts(
                categoryIds: headphoneCategory.FirstOrDefault()?.SubCategories?.Select(x => x.Id).ToList()
            );

            var dac = await _productService.GetAllProducts(
                categoryIds: dacCategory.FirstOrDefault()?.SubCategories?.Select(x => x.Id).ToList()
            );

            if (headphone is not null)
            {
                Headphones = headphone.Items;
            }

            if (dac is not null)
            {
                Dacs = dac.Items;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "Exception occurred while fetching products.");
            throw;
        }
    }
}