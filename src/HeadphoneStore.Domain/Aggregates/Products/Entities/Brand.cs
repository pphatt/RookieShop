using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Aggregates.Products.Entities;

public class Brand : Entity<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    private readonly List<Product> _products = [];
    public virtual IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    protected Brand() { }

    public Brand(string name, string? description, Guid createdBy) : base(Guid.NewGuid())
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        CreatedBy = createdBy;
        CreatedDateTime = DateTime.UtcNow;
    }

    public static Brand Create(string name, string? description, Guid createdBy)
    {
        return new(name, description, createdBy);
    }

    public void Update(string name, string? description, Guid updatedBy)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        UpdatedBy = updatedBy;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void Delete() => IsDeleted = true;
}
