using HeadphoneStore.StoreFrontEnd.Interfaces.Services;

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
    private readonly IAccountService _accountService;

    public LogoutModel(ILogger<LogoutModel> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (!(User.Identity?.IsAuthenticated ?? false)) return RedirectToPage("/Index");

        try
        {
            // Call logout API
            await _accountService.LogoutAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to call API logout endpoint");
        }

        // Remove authentication cookie
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Clear cookies
        Response.Cookies.Delete("AccessToken");
        Response.Cookies.Delete("RefreshToken");

        return RedirectToPage("/Index");
    }
}