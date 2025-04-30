namespace HeadphoneStore.Shared.Services.Identity.CreateUser;

public class CreateUserRequestDto
{
    public string Email { get; set; } = default!;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public Guid RoleId { get; set; }
}
