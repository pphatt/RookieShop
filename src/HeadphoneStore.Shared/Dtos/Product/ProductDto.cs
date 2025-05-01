using HeadphoneStore.Shared.Dtos.Brand;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Shared.Dtos.Product;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public string Sku { get; set; } // Slug
    public string ProductStatus { get; set; } = "In stock";
    public decimal ProductPrice { get; set; }
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public CategoryDto Category { get; set; }
    public BrandDto Brand { get; set; }
    public string Status { get; set; }

    public IReadOnlyCollection<ProductMediaDto> Media { get; set; }
}
