using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.StoreFrontEnd.Pages.Shared.ViewComponents;

public class CategoryMenuViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}
