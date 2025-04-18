using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Category.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
    }
}
