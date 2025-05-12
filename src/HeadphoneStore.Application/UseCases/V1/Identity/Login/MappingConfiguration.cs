using HeadphoneStore.Shared.Services.Identity.Login;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Login;

public static class MappingConfiguration
{
    public static LoginCommand MapToCommand(this LoginRequestDto dto)
        => new(dto.Email, dto.Password);
}
