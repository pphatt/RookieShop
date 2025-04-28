using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Identity.User;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetAllUserPaged;

public class GetAllUserPagedQuery : PagedDto, IQuery<PagedResult<UserDto>>
{
}
