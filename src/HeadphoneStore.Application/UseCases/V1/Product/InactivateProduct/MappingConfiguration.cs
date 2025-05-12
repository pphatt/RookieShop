using HeadphoneStore.Shared.Services.Product.InactivateProduct;

namespace HeadphoneStore.Application.UseCases.V1.Product.InactivateProduct;

public static class MappingConfiguration
{
    public static InactivateProductCommand MapToCommand(this InactivateProductRequestDto dto)
        => new(dto.Id);
}
