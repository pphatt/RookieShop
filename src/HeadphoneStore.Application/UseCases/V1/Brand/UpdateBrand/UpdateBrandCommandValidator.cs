using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
    }
}
