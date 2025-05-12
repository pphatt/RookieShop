using HeadphoneStore.Shared.Services.Brand.Update;

namespace HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;

public static class MappingConfiguration
{
    public static UpdateBrandCommand MapToCommand(this UpdateBrandRequestDto dto)
        => new(dto.Id, dto.Name, dto.Slug, dto.Description, dto.Status);
}
