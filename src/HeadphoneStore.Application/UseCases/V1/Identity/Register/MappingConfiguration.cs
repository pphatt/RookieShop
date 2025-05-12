using HeadphoneStore.Shared.Services.Identity.Register;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Register;

public static class MappingConfiguration
{
    public static RegisterCommand MapToCommand(this RegisterRequestDto dto)
        => new(dto.Email, dto.Password, dto.ConfirmPassword);
}
