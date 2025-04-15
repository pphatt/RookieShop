using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Aggregates.Categories.Entities;

public class Category : Entity<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    public Guid? ParentId { get; private set; }
    public virtual Category? Parent { get; private set; }
    private readonly List<Category> _children = new();
    public virtual IReadOnlyCollection<Category> Children => _children.AsReadOnly();

    private Category() { } // For EF Core

    public Category(string name, string description, Guid createdBy, Category? parent = null) : base(Guid.NewGuid())
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
        Parent = parent;
        ParentId = parent?.Id;
    }

    public void Delete() => IsDeleted = true;

    public void Update(string name, string description, Guid updatedBy)
    {
        if (name.Length > 256)
            throw new ArgumentException("Name cannot exceed 256 characters.");

        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        UpdatedBy = updatedBy;
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public void AddChild(Category category)
    {
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        _children.Add(category);

        category.SetParent(this);
    }

    internal void SetParent(Category parent)
    {
        Parent = parent;
        ParentId = parent?.Id;
    }
}
