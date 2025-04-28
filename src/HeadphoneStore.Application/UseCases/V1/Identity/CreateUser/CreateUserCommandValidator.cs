using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Identity.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
    }
}
