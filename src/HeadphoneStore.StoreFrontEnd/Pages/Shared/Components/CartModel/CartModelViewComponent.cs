using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.StoreFrontEnd.Pages.Shared.Components.CartModel;

public class CartModelViewComponent : ViewComponent
{
    public CartModelViewComponent()
    {
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}
