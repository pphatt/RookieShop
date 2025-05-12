using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.Shared.Services.Identity.DeleteUser;

public class DeleteUserRequestDto
{
    [FromRoute]
    public Guid Id { get; set; }
}
