using HeadphoneStore.Shared.Services.Brand.BulkDelete;

namespace HeadphoneStore.Application.UseCases.V1.Brand.BulkDeleteBrand;

public static class MappingConfiguration
{
    public static BulkDeleteBrandCommand MapToCommand(this BulkDeleteBrandRequestDto dto)
        => new(dto.Ids);
}
