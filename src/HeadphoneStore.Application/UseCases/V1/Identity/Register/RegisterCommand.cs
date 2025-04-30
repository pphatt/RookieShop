using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string ConfirmPassword) : ICommand
{
}
