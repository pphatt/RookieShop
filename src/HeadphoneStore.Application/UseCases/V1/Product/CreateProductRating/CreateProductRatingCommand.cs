using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProductRating;

public sealed record CreateProductRatingCommand(Guid ProductId,
                                                Guid CustomerId,
                                                int RatingValue,
                                                string? Comment) : ICommand
{
}
