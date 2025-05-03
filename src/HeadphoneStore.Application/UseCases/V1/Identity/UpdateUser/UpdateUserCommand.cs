using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Identity.UpdateUser;

public class UpdateUserCommand : ICommand
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public Guid RoleId { get; set; }
    public string Status { get; set; }
}
