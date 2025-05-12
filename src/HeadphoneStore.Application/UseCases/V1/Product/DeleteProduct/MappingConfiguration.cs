using HeadphoneStore.Shared.Services.Product.Delete;

namespace HeadphoneStore.Application.UseCases.V1.Product.DeleteProduct;

public static class MappingConfiguration
{
    public static DeleteProductCommand MapToCommand(this DeleteProductRequestDto dto)
        => new(dto.Id);
}
