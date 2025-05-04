using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Services.Identity.Login;
using HeadphoneStore.StoreFrontEnd.Apis;
using HeadphoneStore.StoreFrontEnd.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeadphoneStore.StoreFrontEnd.Pages.Account;

[ValidateAntiForgeryToken]
public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty] public LoginRequestDto LoginRequest { get; init; } = null!;
    [BindProperty] public bool RememberMe { get; set; }

    public LoginModel(
        ILogger<LoginModel> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public void OnGet(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated ?? false) return LocalRedirect("/Index");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(AuthenticationApi.LoginEndpoint, LoginRequest);
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!response.IsSuccessStatusCode)
            {
                var errorStream = await response.Content.ReadAsStreamAsync();
                var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponse>(errorStream, jsonOptions);

                if (errorResponse?.Errors != null)
                {
                    foreach (var error in errorResponse.Errors)
                    {
                        foreach (var message in error.Value)
                        {
                            ModelState.AddModelError(error.Key, message);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(errorResponse?.Title))
                {
                    ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred during login.");
                }

                return Page();
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var apiResponse = await JsonSerializer
                .DeserializeAsync<Result<LoginResponseDto>>(responseStream, jsonOptions);

            var user = apiResponse!.Value;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(user.AccessToken);
            var claims = jwtToken.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
            
            claims.Add(new Claim("access_token", user.AccessToken));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                RedirectUri = returnUrl ?? "/Index"
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect(returnUrl ?? "/Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred during login");
            ModelState.AddModelError(string.Empty, "An error occurred. Please try again.");
            return Page();
        }
    }
}