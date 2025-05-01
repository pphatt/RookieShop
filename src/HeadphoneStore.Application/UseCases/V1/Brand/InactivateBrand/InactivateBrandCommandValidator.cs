using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Brand.InactiveBrand;

public class InactivateBrandCommandValidator : AbstractValidator<InactivateBrandCommand>
{
    public InactivateBrandCommandValidator()
    {
    }
}
