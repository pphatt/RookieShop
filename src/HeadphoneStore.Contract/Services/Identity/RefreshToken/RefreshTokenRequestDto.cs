namespace HeadphoneStore.Contract.Services.Identity.RefreshToken;

public class RefreshTokenRequestDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
