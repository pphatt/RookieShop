using HeadphoneStore.Application.UseCases.V1.Brand.ActiveBrand;
using HeadphoneStore.Shared.Services.Brand.ActiveBrand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.ActivateBrand;

public static class MappingConfiguration
{
    public static ActivateBrandCommand MapToCommand(this ActivateBrandRequestDto dto)
        => new(dto.Id);
}
