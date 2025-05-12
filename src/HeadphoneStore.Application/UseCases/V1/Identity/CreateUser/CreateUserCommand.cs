using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Identity.CreateUser;

public sealed record CreateUserCommand(string Email,
                                       string FirstName,
                                       string LastName,
                                       string PhoneNumber,
                                       Guid RoleId,
                                       string Status) : ICommand
{
}
