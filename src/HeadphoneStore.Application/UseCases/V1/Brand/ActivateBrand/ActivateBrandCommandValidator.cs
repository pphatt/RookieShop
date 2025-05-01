using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Brand.ActiveBrand;

public class ActivateBrandCommandValidator : AbstractValidator<ActivateBrandCommand>
{
    public ActivateBrandCommandValidator()
    {
    }
}
