using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Services.Identity.Login;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : ICommand<LoginResponseDto>
{
}
