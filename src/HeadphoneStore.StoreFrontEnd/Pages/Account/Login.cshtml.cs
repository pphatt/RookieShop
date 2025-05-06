using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using HeadphoneStore.Shared.Services.Identity.Login;
using HeadphoneStore.StoreFrontEnd.Interfaces.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeadphoneStore.StoreFrontEnd.Pages.Account;

[ValidateAntiForgeryToken]
public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly IAccountService _accountService;

    public LoginModel(
        ILogger<LoginModel> logger,
        IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [BindProperty] public InputModel Input { get; init; } = null!;

    public string? ReturnUrl { get; set; }

    [TempData] public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public void OnGet(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        ReturnUrl = returnUrl ?? Url.Content("~/");
    }

    public async Task<IActionResult> OnPost(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (User.Identity?.IsAuthenticated ?? false) return LocalRedirect(returnUrl);

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty,
                "Invalid login attempt. Please check your credentials and try again.");
            return Page();
        }

        try
        {
            var loginRequest = new LoginRequestDto { Email = Input.Email, Password = Input.Password };

            var result = await _accountService.LoginAsync(loginRequest);

            if (result is null)
            {
                // TODO: handle error but for now is this.
                Console.WriteLine(result);
                ModelState.AddModelError(string.Empty,
                    "Invalid login attempt. Please check your credentials and try again.");
                return Page();
            }

            // Parse JWT token
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.AccessToken);

            // Setup claim and identity
            var claims = token.Claims.ToList();
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principle = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, 
                ExpiresUtc = result.RefreshTokenExpiryTime, 
                RedirectUri = returnUrl
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principle,
                authProperties);

            return LocalRedirect(returnUrl);
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty,
                "Invalid login attempt. Please check your credentials and try again.");
            return Page();
        }
    }
}