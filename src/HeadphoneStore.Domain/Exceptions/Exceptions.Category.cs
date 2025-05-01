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
            description: "Category has already been deleted."
        );

        public sealed class AlreadyActive() : DomainException.ValidationException(
            code: "Category.AlreadyActive",
            description: "Category has already been activated."
        );

        public sealed class AlreadyInactivate() : DomainException.ValidationException(
            code: "Category.AlreadyInactive",
            description: "Category has already been inactivated."
        );

        public sealed class SlugExists() : DomainException.ValidationException(
            code: "Category.SlugExists",
            description: "Please enter another name. Current name is already exist."
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
