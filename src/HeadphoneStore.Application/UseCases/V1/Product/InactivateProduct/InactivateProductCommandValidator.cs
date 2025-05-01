using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Product.InactivateProduct;

public class InactivateProductCommandValidator : AbstractValidator<InactivateProductCommand>
{
    public InactivateProductCommandValidator()
    {
    }
}
