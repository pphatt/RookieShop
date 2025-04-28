using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Identity.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
    }
}
