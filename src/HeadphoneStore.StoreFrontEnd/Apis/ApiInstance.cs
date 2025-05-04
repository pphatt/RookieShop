using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Options;

using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Services.Identity.Authenticated;
using HeadphoneStore.StoreFrontEnd.Apis.Endpoints;
using HeadphoneStore.StoreFrontEnd.Apis.Interfaces;
using HeadphoneStore.StoreFrontEnd.Common.Options;

namespace HeadphoneStore.StoreFrontEnd.Apis;

public class ApiInstance : IApiInstance
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApiOptions _apiOptions;
    private readonly ILogger<ApiInstance> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiInstance(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor,
        IOptions<ApiOptions> apiOptions,
        ILogger<ApiInstance> logger)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _apiOptions = apiOptions!.Value;
        _logger = logger;

        // Configure HttpClient
        _httpClient.BaseAddress = new Uri($"{_apiOptions.BaseUrl}/{_apiOptions.ApiVersion}/");
        _httpClient.Timeout = TimeSpan.FromSeconds(_apiOptions.Timeout);

        // Configure JSON serialization options
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = false
        };
    }

    private void AddAuthorizationHeader()
    {
        var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

        if (!string.IsNullOrEmpty(accessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _logger.LogWarning("No access token found in cookies.");
        }
    }

    private async Task<T?> ExecuteWithRetryAsync<T>(Func<Task<HttpResponseMessage>> requestAction)
    {
        AddAuthorizationHeader();

        var response = await requestAction();

        if (response.IsSuccessStatusCode)
        {
            if (typeof(T) == typeof(bool)) return (T)(object)true;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            bool tokenRefreshed = await RefreshTokenAsync();

            if (tokenRefreshed)
            {
                return await ExecuteWithRetryAsync<T>(requestAction);
            }

            _logger.LogWarning("Token refresh failed.");
        }

        _logger.LogError($"API request failed with status code {response.StatusCode}");

        return default;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        return await ExecuteWithRetryAsync<T>(async () => await _httpClient.GetAsync(endpoint));
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(data, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        return await ExecuteWithRetryAsync<TResponse>(async () => await _httpClient.PostAsync(endpoint, jsonContent));
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(data, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        return await ExecuteWithRetryAsync<TResponse>(async () => await _httpClient.PutAsync(endpoint, jsonContent));
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        await ExecuteWithRetryAsync<bool>(async () => await _httpClient.DeleteAsync(endpoint));

        return true;
    }

    private async Task<bool> RefreshTokenAsync()
    {
        try
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Missing refresh or access token for token refreshing.");
                return false;
            }

            var tokenRequest = new
            {
                AccessToken = accessToken, 
                RefreshToken = refreshToken
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(tokenRequest, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = null; // this can be not necessary (api side still allows anonymous)
            var response = await _httpClient.PostAsync(AuthenticationApi.RefreshTokenEndpoint, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Token refresh request failed with status code {StatusCode}", response.StatusCode);
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<Result<TokenResponse>>(content, _jsonOptions);

            if (tokenResponse?.Value == null)
            {
                _logger.LogWarning("Invalid token refresh response.");
                return false;
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_apiOptions.ExpiryMinutes)
            };

            // Delete expired ones
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("AccessToken");
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("RefreshToken");
            
            // Set new token
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("AccessToken", tokenResponse.Value.AccessToken, cookieOptions);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("RefreshToken", tokenResponse.Value.RefreshToken, cookieOptions);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during token refresh.");
            return false;
        }
    }
}
