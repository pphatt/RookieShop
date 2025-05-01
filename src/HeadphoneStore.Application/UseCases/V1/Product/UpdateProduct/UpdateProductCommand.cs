using HeadphoneStore.Shared.Abstracts.Commands;

using Microsoft.AspNetCore.Http;

namespace HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;

public class UpdateProductCommand : ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public int Stock { get; set; }
    public string Sku { get; set; }
    public string ProductStatus { get; set; }
    public int ProductPrice { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public List<Guid>? OldImages { get; set; }
    public List<IFormFile>? NewImages { get; set; }
    public List<string> ListOrder { get; set; }
    public string Status { get; set; }
    public Guid UpdatedBy { get; set; }
}
