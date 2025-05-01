using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Category.InactivateCategory;

public class InactivateCategoryCommandValidator : AbstractValidator<InactivateCategoryCommand>
{
    public InactivateCategoryCommandValidator()
    {
    }
}
