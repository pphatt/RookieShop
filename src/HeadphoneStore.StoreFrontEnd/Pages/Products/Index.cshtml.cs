using HeadphoneStore.Shared.Dtos.Brand;
using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.StoreFrontEnd.Services.Interfaces;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.StoreFrontEnd.Pages.Products;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IProductService _productService;
    private readonly IBrandService _brandService;
    private readonly ICategoryService _categoryService;

    public IndexModel(ILogger<IndexModel> logger, IProductService productService, IBrandService brandService,
        ICategoryService categoryService)
    {
        _logger = logger;
        _productService = productService;
        _categoryService = categoryService;
        _brandService = brandService;
    }

    public List<ProductDto> Headphones { get; set; } = [];

    public List<BrandDto> Brands { get; set; } = [];

    [BindProperty(SupportsGet = true)] public string SortBy { get; set; } = "title";

    public async Task OnGetAsync()
    {
        try
        {
            var categories = await _categoryService.GetAllCategories(searchTerm: "Tai Nghe");

            var subCategoriesIds = categories.FirstOrDefault()?.SubCategories?.Select(x => x.Id).ToList();

            var brands =
                await _brandService.GetAllBrands(subCategoriesIds);

            var headphone = await _productService.GetAllProducts(
                categoryIds: subCategoriesIds
            );

            if (headphone?.Value is not null)
            {
                Headphones = headphone.Value.Items;
            }

            if (brands is not null)
            {
                Brands = brands;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred while fetching products.");
            throw;
        }
    }
}