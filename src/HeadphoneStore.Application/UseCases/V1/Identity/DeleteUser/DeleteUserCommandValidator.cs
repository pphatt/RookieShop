using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Identity.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
    }
}
