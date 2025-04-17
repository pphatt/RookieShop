namespace HeadphoneStore.Domain.Exceptions;

public static partial class Exceptions
{
    public static class Category
    {
        public sealed class DuplicateName() : DomainException.ValidationException(
            code: "Category.DuplicateName",
            description: "Category's name already exists."
        );

        public sealed class NotFound() : DomainException.NotFoundException(
            code: "Category.NotFound",
            description: "Category was not found."
        );
    }
}
