using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.Shared.Services.Identity.DeleteUser;

public class DeleteUserRequestDto
{
    [FromRoute]
    public string Id { get; set; }
}
