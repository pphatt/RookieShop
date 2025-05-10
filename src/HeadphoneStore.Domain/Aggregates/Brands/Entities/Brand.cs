using HeadphoneStore.Domain.Abstracts.Aggregates;
using HeadphoneStore.Domain.Abstracts.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Enumerations;

namespace HeadphoneStore.Domain.Aggregates.Brands.Entities;

public class Brand : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string Slug { get; private set; }

    private readonly List<Product> _products = [];
    public virtual IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    protected Brand() { }
    protected Brand(string name,
                  string slug,
                  string? description,
                  EntityStatus status = EntityStatus.Active) : base(Guid.NewGuid())
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Slug = slug;
        Description = description;
        CreatedDateTime = DateTime.UtcNow;
        Status = status;
    }

    public static Brand Create(string name, string slug, string? description, EntityStatus status = EntityStatus.Active)
        => new(name, slug, description, status);

    public void Update(string name, string slug, string? description, EntityStatus status)
    {
        Name = name;
        Slug = slug;
        Description = description;
        ModifiedDateTime = DateTime.UtcNow;
        Status = status;
    }
}
