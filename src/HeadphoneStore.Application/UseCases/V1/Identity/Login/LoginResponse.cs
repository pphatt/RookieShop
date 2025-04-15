namespace HeadphoneStore.Application.UseCases.V1.Identity.Login;

public class LoginResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
