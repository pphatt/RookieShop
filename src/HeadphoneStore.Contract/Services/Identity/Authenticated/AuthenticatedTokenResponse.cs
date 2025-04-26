namespace HeadphoneStore.Contract.Services.Identity.Authenticated;

public class AuthenticatedTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
