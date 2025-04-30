using System.Text.Json;

using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;
using HeadphoneStore.StoreFrontEnd.Apis;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.StoreFrontEnd.Pages.Shared.ViewComponents;

public class CategoryMenuViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CategoryMenuViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(CategoryApi.GetAllCategories);

        if (!response.IsSuccessStatusCode)
            return View(new List<CategoryDto>());

        var stream = await response.Content.ReadAsStreamAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var apiResponse = await JsonSerializer.DeserializeAsync<Result<List<CategoryDto>>>(stream, options);

        return View(apiResponse!.Value);
    }
}
