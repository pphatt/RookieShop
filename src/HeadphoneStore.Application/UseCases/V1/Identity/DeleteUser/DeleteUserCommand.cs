using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Identity.DeleteUser;

public sealed record DeleteUserCommand(Guid Id) : ICommand
{
}
