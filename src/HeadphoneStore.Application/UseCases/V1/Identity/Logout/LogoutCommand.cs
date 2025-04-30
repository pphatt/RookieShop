using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Logout;

public class LogoutCommand : ICommand
{
    public string AccessToken { get; set; }
}
