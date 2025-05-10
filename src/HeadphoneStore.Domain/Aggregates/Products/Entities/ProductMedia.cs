using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Aggregates.Products.Entities;

public class ProductMedia : Entity<Guid>
{
    public string ImageUrl { get; private set; }
    public string PublicId { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public int DisplayOrder { get; set; }

    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }

    protected ProductMedia() { }
    protected ProductMedia(Guid productId,
                           string imageUrl,
                           string publicId,
                           string path,
                           string name,
                           int displayOrder) : base(Guid.NewGuid())
    {
        ProductId = productId;
        ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
        PublicId = publicId;
        Path = path;
        Name = name;
        DisplayOrder = displayOrder;
        CreatedDateTime = DateTime.UtcNow;
    }

    public static ProductMedia Create(Guid productId,
                                      string imageUrl,
                                      string publicId,
                                      string path,
                                      string name,
                                      int displayOrder)
        => new(productId, imageUrl, publicId, path, name, displayOrder);

    public void Update(string imageUrl)
    {
        ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
        ModifiedDateTime = DateTime.UtcNow;
    }
}
