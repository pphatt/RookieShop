namespace HeadphoneStore.Domain.Exceptions;

public static partial class UsersException
{
    public sealed class NotFound() : DomainException.NotFoundException(
        code: "User.NotFound",
        description: "User was not found."
    );

    public sealed class DuplicateEmail() : DomainException.ValidationException(
        code: "User.DuplicateEmail",
        description: "User's email already exists."
    );

    public sealed class InactiveOrLockedOut() : DomainException.ValidationException(
        code: "User.InactiveOrLockedOut",
        description: "The account must be confirmed. Please click send confirm email again or contact admin to unlock."
    );

    public sealed class InvalidCredentials() : DomainException.ValidationException(
        code: "User.InvalidCredentials",
        description: "Email or password is incorrect."
    );

    public sealed class FailResetPassword() : DomainException.FailureException(
        code: "User.FailResetPassword",
        description: "Error occurs while resetting password."
    );
}
