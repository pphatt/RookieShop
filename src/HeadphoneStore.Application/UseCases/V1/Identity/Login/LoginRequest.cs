namespace HeadphoneStore.Application.UseCases.V1.Identity.Login;

public sealed record LoginRequest(string Email, string Password)
{
}
