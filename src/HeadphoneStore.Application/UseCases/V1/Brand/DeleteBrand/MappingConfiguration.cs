using HeadphoneStore.Shared.Services.Brand.Delete;

namespace HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;

public static class MappingConfiguration
{
    public static DeleteBrandCommand MapToCommand(this DeleteBrandRequestDto dto)
        => new(dto.Id);
}
