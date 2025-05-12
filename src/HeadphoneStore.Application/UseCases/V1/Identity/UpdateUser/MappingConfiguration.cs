using HeadphoneStore.Shared.Services.Identity.UpdateUser;

namespace HeadphoneStore.Application.UseCases.V1.Identity.UpdateUser;

public static class MappingConfiguration
{
    public static UpdateUserCommand MapToCommand(this UpdateUserRequestDto dto)
        => new(dto.Id, dto.FirstName, dto.LastName, dto.PhoneNumber, dto.RoleId, dto.Status);
}
