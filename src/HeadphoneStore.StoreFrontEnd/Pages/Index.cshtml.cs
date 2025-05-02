using System.Text.Json;

using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;
using HeadphoneStore.StoreFrontEnd.Apis;
using HeadphoneStore.StoreFrontEnd.Constants;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeadphoneStore.StoreFrontEnd.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

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

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGet()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            string headphoneUri = ProductApi.GetProductByCategory
                                  + $"?CategorySlug={_headphoneCategorySlug}"
                                  + $"&{PaginationConstants.PageSizePathParam}={PaginationConstants.PageSize}";
            
            string dacUri = ProductApi.GetProductByCategory
                            + $"?CategorySlug={_dacCategorySlug}"
                            + $"&{PaginationConstants.PageSizePathParam}={PaginationConstants.PageSize}";

            var jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, };

            // Send both requests concurrently
            var headphoneTask = client.GetAsync(headphoneUri);
            var dacTask = client.GetAsync(dacUri);
            await Task.WhenAll(headphoneTask, dacTask);

            // Headphones response
            if (headphoneTask.Result.IsSuccessStatusCode)
            {
                using var stream = await headphoneTask.Result.Content.ReadAsStreamAsync();

                var headphoneResponse = await JsonSerializer
                    .DeserializeAsync<Result<PagedResult<ProductDto>>>(stream, jsonOptions);

                if (headphoneResponse?.IsSuccessful == true)
                    Headphones = headphoneResponse.Value!.Items.ToList();
            }
            
            // Dacs response
            if (dacTask.Result.IsSuccessStatusCode)
            {
                using var stream = await dacTask.Result.Content.ReadAsStreamAsync();

                var dacResponse = await JsonSerializer
                    .DeserializeAsync<Result<PagedResult<ProductDto>>>(stream, jsonOptions);

                if (dacResponse?.IsSuccessful == true)
                    Dacs = dacResponse.Value!.Items.ToList();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "Exception occurred while fetching products.");
            throw;
        }
    }
}