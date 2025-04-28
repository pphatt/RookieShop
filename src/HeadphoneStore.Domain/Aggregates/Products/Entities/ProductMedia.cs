using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Aggregates.Products.Entities;

public class ProductMedia : Entity<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public string ImageUrl { get; private set; }
    public string PublicId { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }

    protected ProductMedia() { } // For EF Core

    public ProductMedia(Guid productId, string imageUrl, string publicId, string path, string name, int order, Guid createdBy) : base(Guid.NewGuid())
    {
        ProductId = productId;
        ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
        PublicId = publicId;
        Path = path;
        Name = name;
        Order = order;
        CreatedBy = createdBy;
        CreatedDateTime = DateTime.UtcNow;
    }

    public void Delete() => IsDeleted = true;

    public void Update(string imageUrl, Guid updatedBy)
    {
        ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
        UpdatedBy = updatedBy;
        UpdatedDateTime = DateTime.UtcNow;
    }
}
