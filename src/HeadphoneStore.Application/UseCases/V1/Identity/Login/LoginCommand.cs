using HeadphoneStore.Contract.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Login;

public class LoginCommand : ICommand<LoginResponse>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
