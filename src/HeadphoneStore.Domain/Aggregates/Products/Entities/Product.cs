using HeadphoneStore.Domain.Abstracts.Aggregates;
using HeadphoneStore.Domain.Abstracts.Entities;
using HeadphoneStore.Domain.Aggregates.Products.ValueObjects;

namespace HeadphoneStore.Domain.Aggregates.Products.Entities;

public class Product : AggregateRoot<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public ProductPrice ProductPrice { get; private set; }
    public string Sku { get; private set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    private readonly List<ProductMedia> _media = [];
    public virtual IReadOnlyCollection<ProductMedia> Media => _media.AsReadOnly();

    private Product() { }

    public Product(
        Guid categoryId,
        string name,
        string description,
        ProductPrice productPrice,
        string sku,
        Guid createdBy) : base(Guid.NewGuid())
    {
        CategoryId = categoryId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ProductPrice = productPrice.Amount >= 0 ? productPrice : throw new ArgumentException("Price cannot be negative.");
        Sku = sku ?? throw new ArgumentNullException(nameof(sku));
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(
        string name,
        string description,
        ProductPrice productPrice,
        string sku,
        Guid updatedBy)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ProductPrice = productPrice.Amount >= 0 ? productPrice : throw new ArgumentException("Price cannot be negative.");
        Sku = sku ?? throw new ArgumentNullException(nameof(sku));
        UpdatedBy = updatedBy;
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public void AddMedia(string imageUrl, Guid createdBy)
    {
        var media = new ProductMedia(imageUrl, createdBy);

        _media.Add(media);

        ModifiedOnUtc = DateTime.UtcNow;
    }

    protected override void EnsureValidState()
    {
    }
}
