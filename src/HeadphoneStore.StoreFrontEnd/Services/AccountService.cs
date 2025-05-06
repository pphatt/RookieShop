using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Services.Identity.Login;
using HeadphoneStore.StoreFrontEnd.Apis.Endpoints;
using HeadphoneStore.StoreFrontEnd.Interfaces.Apis;
using HeadphoneStore.StoreFrontEnd.Interfaces.Services;

namespace HeadphoneStore.StoreFrontEnd.Services;

public class AccountService : IAccountService
{
    private readonly IApiInstance _apiInstance;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AccountService> _logger;

    public AccountService(IApiInstance apiInstance, 
                          IHttpContextAccessor httpContextAccessor,
                          ILogger<AccountService> logger)
    {
        _apiInstance = apiInstance;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto model)
    {
        var response = await _apiInstance.PostAsync<LoginRequestDto, Result<LoginResponseDto>>(AuthenticationApi.LoginEndpoint, model);

        if (response is not null)
        {
            var token = response.Value;

            // Store tokens in cookies
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = token.RefreshTokenExpiryTime
            };

            _httpContextAccessor.HttpContext!.Response.Cookies.Append("AccessToken", token.AccessToken, cookieOptions);
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("RefreshToken", token.RefreshToken, cookieOptions);
        }

        return response.Value;
    }

    public async Task<bool> LogoutAsync()
    {
        var response = await _apiInstance.PostAsync<object, object>(AuthenticationApi.LogoutEndpoint, null!);

        if (response is not null)
        {
            // Clear token in cookies
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("AccessToken");
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("RefreshToken");

            return true;
        }

        return false;
    }
}