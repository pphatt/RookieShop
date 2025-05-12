using HeadphoneStore.Shared.Services.Identity.CreateUser;

namespace HeadphoneStore.Application.UseCases.V1.Identity.CreateUser;

public static class MappingConfiguration
{
    public static CreateUserCommand MapToCommand(this CreateUserRequestDto dto)
        => new(dto.Email, dto.FirstName, dto.LastName, dto.PhoneNumber, dto.RoleId, dto.Status);
}
