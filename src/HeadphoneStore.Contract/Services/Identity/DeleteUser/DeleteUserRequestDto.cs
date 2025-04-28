using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.Contract.Services.Identity.DeleteUser;

public class DeleteUserRequestDto
{
    [FromRoute]
    public string Id { get; set; }
}
