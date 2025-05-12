using HeadphoneStore.Shared.Services.Brand.GetById;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetBrandById;

public static class MappingConfiguration
{
    public static GetBrandByIdQuery MapToQuery(this GetBrandByIdRequestDto dto)
        => new(dto.Id);
}
