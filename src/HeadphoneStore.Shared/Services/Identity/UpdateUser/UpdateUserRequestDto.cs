using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.Shared.Services.Identity.UpdateUser;

public class UpdateUserRequestDto
{
    [FromRoute]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public Guid RoleId { get; set; }
    public string Status { get; set; }
}
