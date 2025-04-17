using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.CreatedBy)
            .NotNull()
            .NotEmpty();
    }
}
