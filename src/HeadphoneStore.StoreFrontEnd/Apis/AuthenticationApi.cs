namespace HeadphoneStore.StoreFrontEnd.Apis;

public class AuthenticationApi
{
    /// <summary>
    /// Sample endpoint: https://localhost:8081/api/v1/Authentication/login
    /// </summary>
    public const string LoginEndpoint = $"{BaseApi._devBaseUrl}/Authentication/login";
    
    /// <summary>
    /// Sample endpoint: https://localhost:8081/api/v1/Authentication/logout
    /// </summary>
    public const string LogoutEndpoint = $"{BaseApi._devBaseUrl}/Authentication/logout";
}