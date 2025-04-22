using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Product.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
    }
}
