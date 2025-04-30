using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Identity.User;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetUserById;

public class GetUserByIdQuery : IQuery<UserDto>
{
    public Guid Id { get; set; }
}
