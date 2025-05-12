using HeadphoneStore.Shared.Services.Product.Update;

namespace HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;

public static class MappingConfiguration
{
    public static UpdateProductCommand MapToCommand(this UpdateProductRequestDto dto)
        => new(dto.Id,
               dto.Name,
               dto.Slug,
               dto.Description,
               dto.Stock,
               dto.Sku,
               dto.ProductStatus,
               dto.ProductPrice,
               dto.CategoryId,
               dto.BrandId,
               dto.OldImages,
               dto.NewImages,
               dto.ListOrder,
               dto.Status);
}
