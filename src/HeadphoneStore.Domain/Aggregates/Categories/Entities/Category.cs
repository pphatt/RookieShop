using HeadphoneStore.Domain.Abstracts.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Entities;

namespace HeadphoneStore.Domain.Aggregates.Categories.Entities;

public class Category : Entity<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    public Guid? ParentId { get; private set; }
    public virtual Category? Parent { get; private set; }
    private readonly List<Category> _subCategories = new();
    public virtual IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();
    private readonly List<Product> _products = new();
    public virtual IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    protected Category() { }

    public Category(string name, string description, Guid createdBy, Category? parent = null) : base(Guid.NewGuid())
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        CreatedBy = createdBy;
        CreatedDateTime  = DateTime.UtcNow;
        Parent = parent;
        ParentId = parent?.Id;
    }

    public static Category Create(string name, string description, Guid createdBy, Category? parent = null)
    {
        return new(name, description, createdBy, parent);
    }

    public void Delete() => IsDeleted = true;

    public void Update(string name, string description, Category? parent, Guid updatedBy)
    {
        if (name.Length > 256)
            throw new ArgumentException("Name cannot exceed 256 characters.");

        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        UpdatedBy = updatedBy;
        Parent = parent;
        UpdatedDateTime = DateTime.UtcNow;
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
