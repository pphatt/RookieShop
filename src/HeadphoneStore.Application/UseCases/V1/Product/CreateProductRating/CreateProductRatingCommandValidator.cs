using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProductRating;

public class CreateProductRatingCommandValidator : AbstractValidator<CreateProductRatingCommand>
{
    public CreateProductRatingCommandValidator()
    {
    }
}
