using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Quantity)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);
    }
}
