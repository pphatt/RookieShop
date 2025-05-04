namespace HeadphoneStore.StoreFrontEnd.Apis.Endpoints;

public class AuthenticationApi
{
    /// <summary>
    /// [AllowAnonymous]
    /// <br />
    /// Description: Login api endpoint.
    /// <br />
    /// Sample endpoint: https://localhost:8081/api/v1/Authentication/login
    /// </summary>
    public const string LoginEndpoint = "Authentication/login";
    
    /// <summary>
    /// [AllowAnonymous]
    /// <br />
    /// Description: Logout api endpoint.
    /// <br />
    /// Sample endpoint: https://localhost:8081/api/v1/Authentication/logout
    /// </summary>
    public const string LogoutEndpoint = "Authentication/logout";
    
    /// <summary>
    /// [AllowAnonymous]
    /// <br />
    /// Description: Refresh token api endpoint.
    /// <br />
    /// Sample endpoint: https://localhost:8081/api/v1/Authentication/refresh-token
    /// </summary>
    public const string RefreshTokenEndpoint = "Authentication/refresh-token";
}