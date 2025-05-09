namespace HeadphoneStore.Shared.Dtos.Product;

public class ProductRatingDto
{
    public string CustomerAvatar { get; set; }
    public string CustomerName { get; set; }
    public string RatingValue { get; set; }
    public string Comment { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
