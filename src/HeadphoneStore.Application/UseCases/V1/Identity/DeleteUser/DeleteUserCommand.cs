using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Identity.DeleteUser;

public class DeleteUserCommand : ICommand
{
    public Guid Id { get; set; }
}
