using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Identity.User;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetAllUserPaged;

public class GetAllUserPagedQuery : PagedDto, IQuery<PagedResult<UserDto>>
{
}
