using HeadphoneStore.Shared.Services.Product.ActivateProduct;

namespace HeadphoneStore.Application.UseCases.V1.Product.ActivateProduct;

public static class MappingConfiguration
{
    public static ActivateProductCommand MapToCommand(this ActivateProductRequestDto dto)
        => new(dto.Id);
}
