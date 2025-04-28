using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Dtos.Identity.User;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetUserById;

public class GetUserByIdQuery : IQuery<UserDto>
{
    public Guid Id { get; set; }
}
