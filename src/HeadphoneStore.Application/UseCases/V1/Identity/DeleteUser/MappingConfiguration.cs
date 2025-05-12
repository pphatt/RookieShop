using HeadphoneStore.Shared.Services.Identity.DeleteUser;

namespace HeadphoneStore.Application.UseCases.V1.Identity.DeleteUser;

public static class MappingConfiguration
{
    public static DeleteUserCommand MapToCommand(this DeleteUserRequestDto dto)
        => new(dto.Id);
}
