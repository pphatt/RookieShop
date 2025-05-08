namespace HeadphoneStore.Shared.Services.ProductRating.CreateProductRating;

public class CreateProductRatingRequestDto
{
    public Guid ProductId { get; set; }
    public int RatingValue { get; set; }
    public string? Comment { get; set; }
}
