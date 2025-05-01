using FluentValidation;

using HeadphoneStore.Domain.Aggregates.Products.Enumerations;
using HeadphoneStore.Domain.Enumerations;

namespace HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
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

        RuleFor(x => x.ProductStatus)
            .NotEmpty()
            .NotNull()
            .Must(BeValidProductEnum)
            .WithMessage($"Invalid Status value. Valid values are: {string.Join(", ", Enum.GetNames(typeof(ProductStatus)))}.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .NotNull()
            .Must(BeValidEnum)
            .WithMessage($"Invalid Status value. Valid values are: {string.Join(", ", Enum.GetNames(typeof(EntityStatus)))}.");
    }

    private bool BeValidProductEnum(string status)
    {
        return Enum.TryParse<ProductStatus>(status, true, out _);
    }

    private bool BeValidEnum(string status)
    {
        return Enum.TryParse<EntityStatus>(status, ignoreCase: true, out _);
    }
}
