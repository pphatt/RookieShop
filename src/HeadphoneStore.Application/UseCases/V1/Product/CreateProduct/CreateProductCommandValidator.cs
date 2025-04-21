using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
    }
}
