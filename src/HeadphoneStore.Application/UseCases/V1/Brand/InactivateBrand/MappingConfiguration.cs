using HeadphoneStore.Application.UseCases.V1.Brand.InactiveBrand;
using HeadphoneStore.Shared.Services.Brand.InactiveBrand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.InactivateBrand;

public static class MappingConfiguration
{
    public static InactivateBrandCommand MapToCommand(this InactivateBrandRequestDto dto)
        => new(dto.Id);
}
