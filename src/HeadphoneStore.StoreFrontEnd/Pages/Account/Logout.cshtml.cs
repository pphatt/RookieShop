using System.Net.Http.Headers;

using HeadphoneStore.StoreFrontEnd.Apis;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeadphoneStore.StoreFrontEnd.Pages.Account;

[Authorize]
[ValidateAntiForgeryToken]
public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public LogoutModel(ILogger<LogoutModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnPost()
    {
        if (!(User.Identity?.IsAuthenticated ?? false)) return RedirectToPage("/Index");

        try
        {
            var accessToken = User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(accessToken))
            {
                var client = _httpClientFactory.CreateClient();
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                
                await client.PostAsync(AuthenticationApi.LogoutEndpoint, null);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to call API logout endpoint");
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToPage("/Index");
    }
}