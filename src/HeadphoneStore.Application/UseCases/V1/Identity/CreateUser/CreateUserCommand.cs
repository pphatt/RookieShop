using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Identity.CreateUser;

public class CreateUserCommand : ICommand
{
    public string Email { get; set; } = default!;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public Guid RoleId { get; set; }
}
