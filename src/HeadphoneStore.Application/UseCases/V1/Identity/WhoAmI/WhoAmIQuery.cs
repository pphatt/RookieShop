using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Identity.User;

namespace HeadphoneStore.Application.UseCases.V1.Identity.WhoAmI;

public class WhoAmIQuery : IQuery<UserDto>
{
    public string UserId { get; set; }
}
