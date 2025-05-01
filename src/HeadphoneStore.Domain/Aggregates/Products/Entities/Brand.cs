using HeadphoneStore.Domain.Abstracts.Entities;
using HeadphoneStore.Domain.Enumerations;

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

    public Brand(string name, string? description, Guid createdBy, EntityStatus status = EntityStatus.Active) : base(Guid.NewGuid())
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        CreatedBy = createdBy;
        CreatedDateTime = DateTime.UtcNow;
        Status = status;
    }

    public static Brand Create(string name, string? description, Guid createdBy, EntityStatus status = EntityStatus.Active)
    {
        return new(name, description, createdBy, status);
    }

    public void Update(string name, string? description, Guid updatedBy, EntityStatus status)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        UpdatedBy = updatedBy;
        UpdatedDateTime = DateTime.UtcNow;
        Status = status;
    }
}
