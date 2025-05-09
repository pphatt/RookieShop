using HeadphoneStore.Domain.Abstracts.Aggregates;
using HeadphoneStore.Domain.Aggregates.Brands.Entities;
using HeadphoneStore.Domain.Aggregates.Categories.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Enumerations;
using HeadphoneStore.Domain.Aggregates.Products.ValueObjects;
using HeadphoneStore.Domain.Enumerations;

namespace HeadphoneStore.Domain.Aggregates.Products.Entities;

public class Product : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Stock { get; private set; }
    public string Sku { get; private set; }
    public string Slug { get; private set; }
    public int Sold { get; private set; }
    public ProductStatus ProductStatus { get; private set; } = ProductStatus.InStock;
    public ProductPrice ProductPrice { get; private set; } = ProductPrice.CreateEmpty();
    public double AverageRating { get; private set; }
    public int TotalReviews { get; private set; }

    public Guid CategoryId { get; private set; }
    public virtual Category Category { get; private set; }

    public Guid BrandId { get; private set; }
    public virtual Brand Brand { get; private set; }

    private readonly List<ProductMedia> _media = [];
    public virtual IReadOnlyCollection<ProductMedia> Media => _media.AsReadOnly();

    private readonly List<ProductRating> _ratings = [];
    public virtual IReadOnlyCollection<ProductRating> Ratings => _ratings.AsReadOnly();

    protected Product() { }
    protected Product(string name,
                      string description,
                      int stock,
                      ProductStatus productStatus,
                      ProductPrice productPrice,
                      string sku,
                      string slug,
                      int sold,
                      Category category,
                      Brand brand,
                      EntityStatus status = EntityStatus.Active) : base(Guid.NewGuid())
    {
        Name = name;
        Description = description;
        Stock = stock;
        ProductStatus = productStatus;
        ProductPrice = productPrice;
        Sku = sku;
        Slug = slug;
        Sold = sold;
        Category = category;
        Brand = brand;
        Status = status;
        CreatedDateTime = DateTimeOffset.UtcNow;
    }

    public static Product Create(string name,
                                 string description,
                                 int stock,
                                 ProductStatus productStatus,
                                 ProductPrice productPrice,
                                 string sku,
                                 string slug,
                                 int sold,
                                 Category category,
                                 Brand brand,
                                 EntityStatus status = EntityStatus.Active)
        => new(name, description, stock, productStatus, productPrice, sku, slug, sold, category, brand, status);

    public void Update(
        string name,
        string description,
        int stock,
        ProductStatus productStatus,
        ProductPrice productPrice,
        string sku,
        string slug,
        int sold,
        Category category,
        Brand brand,
        EntityStatus status)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Stock = stock;
        ProductStatus = productStatus;
        ProductPrice = productPrice.Amount >= 0 ? productPrice : throw new ArgumentException("Price cannot be negative.");
        Sku = sku ?? throw new ArgumentNullException(nameof(sku));
        Slug = slug;
        Sold = sold;
        Category = category;
        Brand = brand;
        Status = status;
        ModifiedDateTime = DateTimeOffset.UtcNow;
    }

    public void AddMedia(ProductMedia media)
    {
        _media.Add(media);
        ModifiedDateTime = DateTimeOffset.UtcNow;
    }

    public void RemoveMedia(ProductMedia media)
    {
        _media.Remove(media);
        ModifiedDateTime = DateTimeOffset.UtcNow;
    }

    public void AddRating(ProductRating rating, double averageRating, int totalReviews)
    {
        _ratings.Add(rating);

        AverageRating = averageRating;
        TotalReviews = totalReviews;
    }

    protected override void EnsureValidState()
    {
    }
}
