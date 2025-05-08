using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.ProductRating.CreateProductRating;

public class CreateProductRatingCommand : ICommand
{
    public Guid ProductId { get; set; }
    public Guid CustomerId { get; set; }
    public int RatingValue { get; set; }
    public string? Comment { get; set; }
}
