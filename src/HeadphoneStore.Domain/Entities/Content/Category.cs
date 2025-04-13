using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Entities.Content;

public class Category : Entity<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    private Category() { } // For EF Core

    public Category(string name, string description, Guid createdBy) : base(Guid.NewGuid())
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Update(string name, string description, Guid updatedBy)
    {
        if (name.Length > 256)
            throw new ArgumentException("Name cannot exceed 256 characters.");

        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        UpdatedBy = updatedBy;
        ModifiedOnUtc = DateTime.UtcNow;
    }
}
