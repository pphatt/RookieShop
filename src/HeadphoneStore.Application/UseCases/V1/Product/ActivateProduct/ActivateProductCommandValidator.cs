using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Product.ActivateProduct;

public class ActivateProductCommandValidator : AbstractValidator<ActivateProductCommand>
{
    public ActivateProductCommandValidator()
    {
    }
}
