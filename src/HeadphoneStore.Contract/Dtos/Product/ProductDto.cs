namespace HeadphoneStore.Contract.Dtos.Product;

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
    public string CategoryName { get; set; }
    public string BrandName { get; set; }

    public IReadOnlyCollection<ProductMediaDto> Media { get; set; }
}
