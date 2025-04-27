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

        public sealed class AlreadyDeleted() : DomainException.ValidationException(
            code: "Category.AlreadyDeleted",
            description: "Category already deleted."
        );

        public sealed class HasSubCategories() : DomainException.ValidationException(
            code: "Category.HasSubCategories",
            description: "Category which has sub-categories cannot have parent."
        );

        public sealed class CannotReferenceThemself() : DomainException.ValidationException(
            code: "Category.CannotReferenceThemself",
            description: "Category cannot reference themself."
        );
    }
}
