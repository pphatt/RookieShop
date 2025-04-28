using Microsoft.AspNetCore.Http;

namespace HeadphoneStore.Contract.Services.Product.Create;

public class CreateProductRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public string ProductStatus { get; set; }
    public int ProductPrice { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public List<IFormFile>? Images { get; set; }
}
