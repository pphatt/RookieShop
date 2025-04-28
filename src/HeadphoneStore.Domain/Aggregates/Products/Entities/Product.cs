using System.ComponentModel.DataAnnotations;

using HeadphoneStore.Domain.Abstracts.Aggregates;
using HeadphoneStore.Domain.Abstracts.Entities;
using HeadphoneStore.Domain.Aggregates.Categories.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Enumerations;
using HeadphoneStore.Domain.Aggregates.Products.ValueObjects;

namespace HeadphoneStore.Domain.Aggregates.Products.Entities;

public class Product : AggregateRoot<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; set; }
    public string Sku { get; private set; } // Slug
    public ProductStatus ProductStatus { get; private set; } = ProductStatus.InStock;
    public ProductPrice ProductPrice { get; private set; } = ProductPrice.CreateEmpty();
    public double AverageRating { get; private set; }
    public int TotalReviews { get; private set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    public Guid CategoryId { get; set; }
    public virtual Category Category { get; private set; }
    public Guid BrandId { get; set; }
    public virtual Brand Brand { get; private set; }

    private readonly List<ProductMedia> _media = [];
    public virtual IReadOnlyCollection<ProductMedia> Media => _media.AsReadOnly();

    protected Product() { }

    public Product(
        string name,
        string description,
        ProductStatus productStatus,
        ProductPrice productPrice,
        string sku,
        Category category,
        Brand brand,
        Guid createdBy) : base(Guid.NewGuid())
    {
        Name = name;
        Description = description;
        ProductStatus = productStatus;
        ProductPrice = productPrice;
        Sku = sku;
        Category = category;
        Brand = brand;
        CreatedBy = createdBy;
        CreatedDateTime = DateTime.UtcNow;
    }

    public void Delete() => IsDeleted = true;

    public void Update(
        string name,
        string description,
        ProductStatus productStatus,
        ProductPrice productPrice,
        string sku,
        Category category,
        Brand brand,
        Guid updatedBy)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ProductStatus = productStatus;
        ProductPrice = productPrice.Amount >= 0 ? productPrice : throw new ArgumentException("Price cannot be negative.");
        Sku = sku ?? throw new ArgumentNullException(nameof(sku));
        Category = category;
        Brand = brand;
        UpdatedBy = updatedBy;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void AddMedia(ProductMedia media)
    {
        _media.Add(media);
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void RemoveMedia(ProductMedia media)
    {
        _media.Remove(media);
        UpdatedDateTime = DateTime.UtcNow;
    }

    protected override void EnsureValidState()
    {
    }
}
