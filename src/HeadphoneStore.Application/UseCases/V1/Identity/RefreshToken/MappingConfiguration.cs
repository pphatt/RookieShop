using HeadphoneStore.Shared.Services.Identity.RefreshToken;

namespace HeadphoneStore.Application.UseCases.V1.Identity.RefreshToken;

public static class MappingConfiguration
{
    public static RefreshTokenCommand MapToCommand(this RefreshTokenRequestDto dto)
        => new() { AccessToken = dto.AccessToken, RefreshToken = dto.RefreshToken };
}
