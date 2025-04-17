using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Category.DeleteCategory;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
    }
}
