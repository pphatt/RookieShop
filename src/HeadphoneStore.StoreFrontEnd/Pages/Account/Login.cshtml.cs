using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Services.Identity.Login;
using HeadphoneStore.StoreFrontEnd.Apis.Endpoints;
using HeadphoneStore.StoreFrontEnd.Models;
using HeadphoneStore.StoreFrontEnd.Services.Interfaces;

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

    [BindProperty]
    public InputModel Input { get; init; } = null!;

    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

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
            ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your credentials and try again.");
            return Page();
        }

        try
        {
            //var client = _httpClientFactory.CreateClient();
            //var response = await client.PostAsJsonAsync(AuthenticationApi.LoginEndpoint, LoginRequest);
            //var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var loginRequest = new LoginRequestDto
            {
                Email = Input.Email,
                Password = Input.Password
            };

            var result = await _accountService.LoginAsync(loginRequest);

            if (result is null)
            {
                //var errorStream = await response.Content.ReadAsStreamAsync();
                //var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponse>(errorStream, jsonOptions);

                //if (errorResponse?.Errors != null)
                //{
                //    foreach (var error in errorResponse.Errors)
                //    {
                //        foreach (var message in error.Value)
                //        {
                //            ModelState.AddModelError(error.Key, message);
                //        }
                //    }
                //}
                //else if (!string.IsNullOrEmpty(errorResponse?.Title))
                //{
                //    ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "An error occurred during login.");
                //}

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
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect(returnUrl);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred during login");
            ModelState.AddModelError(string.Empty, "An error occurred. Please try again.");
            return Page();
        }
    }
}