using HeadphoneStore.Shared.Abstracts.Commands;

using Microsoft.AspNetCore.Http;

namespace HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Slug,
    string Description,
    int Stock,
    string Sku,
    string ProductStatus,
    int ProductPrice,
    Guid CategoryId,
    Guid BrandId,
    List<Guid>? OldImages,
    List<IFormFile>? NewImages,
    List<string> ListOrder,
    string Status) : ICommand
{
}
