using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Identity.User;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetAllUsersPaged;

public sealed record GetAllUsersPagedQuery(string? SearchTerm,
                                          int PageIndex,
                                          int PageSize) : IQuery<PagedResult<UserDto>>
{
}
