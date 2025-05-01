using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Category.ActivateCategory;

public class ActivateCategoryCommandValidator : AbstractValidator<ActivateCategoryCommand>
{
    public ActivateCategoryCommandValidator()
    {
    }
}
