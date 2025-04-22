using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Domain.Aggregates.Products.Enumerations;

using Microsoft.AspNetCore.Http;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;

public class CreateProductCommand : ICommand
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public int Sku { get; set; }
    public ProductStatus ProductStatus { get; set; }
    public int ProductPrice { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public List<IFormFile>? Files { get; set; }
    public Guid CreatedBy { get; set; }
}
