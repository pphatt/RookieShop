using HeadphoneStore.Domain.Abstracts.Aggregates;
using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Entities.Content;

public class Product : AggregateRoot<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
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
        decimal price,
        string sku,
        Guid createdBy) : base(Guid.NewGuid())
    {
        CategoryId = categoryId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Price = price >= 0 ? price : throw new ArgumentException("Price cannot be negative.");
        Sku = sku ?? throw new ArgumentNullException(nameof(sku));
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(
        string name,
        string description,
        decimal price,
        string sku,
        Guid updatedBy)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Price = price >= 0 ? price : throw new ArgumentException("Price cannot be negative.");
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
