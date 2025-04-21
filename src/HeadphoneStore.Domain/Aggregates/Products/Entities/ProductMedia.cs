using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Aggregates.Products.Entities;

public class ProductMedia : Entity<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public string ImageUrl { get; private set; }
    public string PublicId { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    private ProductMedia() { } // For EF Core

    public ProductMedia(string imageUrl, string publicId, string path, string name, Guid createdBy) : base(Guid.NewGuid())
    {
        ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
        PublicId = publicId;
        Path = path;
        Name = name;
        CreatedBy = createdBy;
        CreatedDateTime  = DateTime.UtcNow;
    }

    public void Delete() => IsDeleted = true;

    public void Update(string imageUrl, Guid updatedBy)
    {
        ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
        UpdatedBy = updatedBy;
        UpdatedDateTime = DateTime.UtcNow;
    }
}
