using HeadphoneStore.Shared.Dtos.Identity.Role;

namespace HeadphoneStore.Shared.Dtos.Identity.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTimeOffset? DayOfBirth { get; set; }
    public string Avatar { get; set; }
    public string Bio { get; set; }
    public string UserStatus { get; set; }
    public IEnumerable<UserAddressDto> UserAddress { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }
    public DateTimeOffset? UpdatedDateTime { get; set; }
    public IEnumerable<RoleDto> Roles { get; set; }
}

public class UserAddressDto
{
    public string Address { get; set; }
    public string Street { get; set; }
    public string Province { get; set; }
    public string PhoneNumber { get; set; }
}
