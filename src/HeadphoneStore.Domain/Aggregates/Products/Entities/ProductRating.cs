using HeadphoneStore.Domain.Abstracts.Entities;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;

namespace HeadphoneStore.Domain.Aggregates.Products.Entities;

public class ProductRating : Entity<Guid>
{
    public int RatingValue { get; set; }
    public string? Comment { get; set; }

    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public Guid CustomerId { get; set; }
    public virtual AppUser Customer { get; set; }

    public static ProductRating Create(Guid productId, Guid customerId, int ratingValue, string? comment)
    {
        return new ProductRating
        {
            ProductId = productId,
            CustomerId = customerId,
            RatingValue = ratingValue,
            Comment = comment
        };
    }

    public void Update(int? ratingValue, string? comment)
    {
        if (ratingValue != null && !ratingValue.Equals(RatingValue)) { RatingValue = (int)ratingValue; }
        if (comment != null && comment != Comment) { Comment = comment; }
    }
}
