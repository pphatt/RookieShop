using FluentValidation;

using HeadphoneStore.Domain.Enumerations;

namespace HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Status)
            .NotEmpty()
            .NotNull()
            .Must(BeValidEnum)
            .WithMessage($"Invalid Status value. Valid values are: {string.Join(", ", Enum.GetNames(typeof(EntityStatus)))}.");
    }

    private bool BeValidEnum(string status)
    {
        return Enum.TryParse<EntityStatus>(status, ignoreCase: true, out _);
    }
}
