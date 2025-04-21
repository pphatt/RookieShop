using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;

public class DeleteBrandCommandValidator : AbstractValidator<DeleteBrandCommand>
{
    public DeleteBrandCommandValidator()
    {
    }
}
