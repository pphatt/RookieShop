using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.Contract.Services.Product.Update;

public class UpdateProductRequestDto
{
    [FromRoute]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public string ProductStatus { get; set; }
    public int ProductPrice { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public List<Guid>? OldImages { get; set; } = new();
    public List<IFormFile>? NewImages { get; set; } = new();
    public List<string> ListOrder { get; set; } = new();
}
