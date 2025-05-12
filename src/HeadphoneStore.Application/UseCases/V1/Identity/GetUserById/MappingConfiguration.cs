using HeadphoneStore.Shared.Services.Identity.GetUserById;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetUserById;

public static class MappingConfiguration
{
    public static GetUserByIdQuery MapToQuery(this GetUserByIdRequestDto dto)
        => new(dto.Id);
}
