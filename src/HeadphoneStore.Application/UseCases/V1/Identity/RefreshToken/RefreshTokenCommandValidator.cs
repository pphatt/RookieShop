using FluentValidation;

namespace HeadphoneStore.Application.UseCases.V1.Identity.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
    }
}
