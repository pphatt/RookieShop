using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Shared.Services.Product.Create;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;

public static class MappingConfiguration
{
    public static CreateProductCommand MapToCommand(this CreateProductRequestDto dto)
        => new(dto.Name,
               Slug: dto.Slug ?? dto.Name.Slugify(),
               dto.Description,
               dto.Stock,
               Sku: dto.Name.Slugify(),
               dto.ProductStatus,
               dto.ProductPrice,
               dto.CategoryId,
               dto.BrandId,
               dto.Images,
               dto.Status);
}
