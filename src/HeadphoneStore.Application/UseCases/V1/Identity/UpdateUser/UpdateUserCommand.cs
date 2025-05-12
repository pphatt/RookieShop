using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Identity.UpdateUser;

public sealed record UpdateUserCommand(Guid Id,
                                       string FirstName,
                                       string LastName,
                                       string PhoneNumber,
                                       Guid RoleId,
                                       string Status) : ICommand
{
}
