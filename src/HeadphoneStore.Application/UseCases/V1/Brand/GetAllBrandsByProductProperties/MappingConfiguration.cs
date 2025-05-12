using HeadphoneStore.Shared.Services.Brand.GetAllBrandsByProductProperties;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsByProductProperties;

public static class MappingConfiguration
{
    public static GetAllBrandsByProductPropertiesQuery MapToQuery(this GetAllBrandsByProductPropertiesRequestDto dto)
        => new(dto.CategoryIds);
}
