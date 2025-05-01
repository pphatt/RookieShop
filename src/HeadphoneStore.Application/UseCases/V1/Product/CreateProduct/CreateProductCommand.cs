using HeadphoneStore.Domain.Aggregates.Products.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;

using Microsoft.AspNetCore.Http;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;

public class CreateProductCommand : ICommand
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public string Sku { get; set; }
    public string ProductStatus { get; set; }
    public int ProductPrice { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public List<IFormFile>? Images { get; set; }
    public string? Status { get; set; }
    public Guid CreatedBy { get; set; }
}
