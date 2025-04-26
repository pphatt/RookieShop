namespace HeadphoneStore.Domain.Exceptions;

public static partial class Exceptions
{
    public static class Token
    {
        public sealed class NotFoundInCached() : DomainException.NotFoundException(
            code: "Token.NotFoundInCached",
            description: "Token is not found in cached. User could be logged out already."
        );

        public sealed class EmailKeyNotFound() : DomainException.NotFoundException(
            code: "Token.EmailKeyNotFound",
            description: "Email key cannot be found in token."
        );
    }
}
