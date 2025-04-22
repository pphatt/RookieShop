using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Product.DeleteProduct;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
    }
}
