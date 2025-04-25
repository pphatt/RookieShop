using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Dtos.Identity.User;

namespace HeadphoneStore.Application.UseCases.V1.Identity.WhoAmI;

public class WhoAmIQuery : IQuery<UserDto>
{
    public string UserId { get; set; }
}
