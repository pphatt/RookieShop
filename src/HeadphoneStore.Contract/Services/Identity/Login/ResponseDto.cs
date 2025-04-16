namespace HeadphoneStore.Contract.Services.Identity.Login;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
