using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.ProductRating.CreateProductRating;

public class CreateProductRatingCommandValidator : AbstractValidator<CreateProductRatingCommand>
{
    public CreateProductRatingCommandValidator()
    {
    }
}
