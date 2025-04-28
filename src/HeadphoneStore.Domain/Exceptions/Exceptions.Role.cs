namespace HeadphoneStore.Domain.Exceptions;

public static partial class Exceptions
{
    public static class Role
    {
        public sealed class NotFound() : DomainException.NotFoundException(
            code: "Role.NotFound",
            description: "Role was not found."
        );
    }
}
