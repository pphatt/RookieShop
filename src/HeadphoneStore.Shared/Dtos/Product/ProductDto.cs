using HeadphoneStore.Shared.Dtos.Brand;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Shared.Dtos.Product;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public int Stock { get; set; }
    public int Sold { get; set; }
    public string Sku { get; set; }
    public string ProductStatus { get; set; } = "In stock";
    public decimal ProductPrice { get; set; }
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public CategoryDto Category { get; set; }
    public BrandDto Brand { get; set; }
    public string Status { get; set; }

    public IReadOnlyCollection<ProductMediaDto> Media { get; set; }
}
