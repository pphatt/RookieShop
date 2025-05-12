using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Identity.User;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<UserDto>
{
}
