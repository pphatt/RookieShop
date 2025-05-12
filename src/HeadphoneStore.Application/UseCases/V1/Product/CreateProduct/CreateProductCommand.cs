using HeadphoneStore.Shared.Abstracts.Commands;

using Microsoft.AspNetCore.Http;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string? Slug,
    string Description,
    int Stock,
    string Sku,
    string ProductStatus,
    int ProductPrice,
    Guid CategoryId,
    Guid BrandId,
    List<IFormFile>? Images,
    string? Status) : ICommand
{
}
