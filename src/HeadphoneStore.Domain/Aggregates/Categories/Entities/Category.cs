using HeadphoneStore.Domain.Abstracts.Aggregates;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Enumerations;

namespace HeadphoneStore.Domain.Aggregates.Categories.Entities;

public class Category : AggregateRoot<Guid>
{
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public string Slug { get; private set; } = "";

    public Guid? ParentId { get; private set; }
    public virtual Category? Parent { get; private set; }

    private readonly List<Category> _subCategories = new();
    public virtual IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

    private readonly List<Product> _products = new();
    public virtual IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    protected Category() { }
    protected Category(string name,
                     string slug,
                     string description,
                     Category? parent = null,
                     EntityStatus status = EntityStatus.Active) : base(Guid.NewGuid())
    {
        Name = name;
        Slug = slug;
        Description = description;
        CreatedDateTime = DateTime.UtcNow;
        Parent = parent;
        ParentId = parent?.Id;
        Status = status;
    }

    public static Category Create(string name,
                                  string slug,
                                  string description,
                                  Category? parent = null,
                                  EntityStatus status = EntityStatus.Active)
        => new(name, slug, description, parent, status);

    public void Update(string name,
                       string slug,
                       string description,
                       Category? parent,
                       EntityStatus status)
    {
        if (name.Length > 256)
            throw new ArgumentException("Name cannot exceed 256 characters.");

        Name = name;
        Slug = slug;
        Description = description;
        Parent = parent;
        ParentId = parent is not null ? parent.Id : null;
        Status = status;
    }

    public void AddSubCategory(Category category)
    {
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        _subCategories.Add(category);

        category.SetParent(this);
    }

    internal void SetParent(Category parent)
    {
        Parent = parent;
        ParentId = parent?.Id;
    }
}
