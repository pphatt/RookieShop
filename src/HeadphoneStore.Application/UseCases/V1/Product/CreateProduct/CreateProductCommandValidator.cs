using FluentValidation;

using HeadphoneStore.Domain.Aggregates.Products.Enumerations;

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

        RuleFor(x => x.Stock)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.ProductStatus)
            .NotEmpty()
            .NotNull()
            .Must(BeValidProductEnum)
            .WithMessage($"Invalid Status value. Valid values are: {string.Join(", ", Enum.GetNames(typeof(ProductStatus)))}.");
    }

    private bool BeValidProductEnum(string status)
    {
        return Enum.TryParse<ProductStatus>(status, true, out _);
    }
}
