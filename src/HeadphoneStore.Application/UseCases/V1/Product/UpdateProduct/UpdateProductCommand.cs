using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Domain.Aggregates.Products.Enumerations;

using Microsoft.AspNetCore.Http;

namespace HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;

public class UpdateProductCommand : ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public int Sku { get; set; }
    public ProductStatus ProductStatus { get; set; }
    public int ProductPrice { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public List<Guid>? OldFiles { get; set; }
    public List<IFormFile>? NewFiles { get; set; }
    public Guid UpdatedBy { get; set; }
}
