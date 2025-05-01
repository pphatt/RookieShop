namespace HeadphoneStore.Domain.Exceptions;

public static partial class Exceptions
{
    public static class Product
    {
        public sealed class NotFound() : DomainException.NotFoundException(
            code: "Product.NotFound",
            description: "Product cannot be found."
        );

        public sealed class DuplicateName() : DomainException.ValidationException(
            code: "Product.DuplicateName",
            description: "Product's name already exists."
        );

        public sealed class AlreadyDeleted() : DomainException.ValidationException(
            code: "Product.AlreadyDeleted",
            description: "Product has already been deleted."
        );

        public sealed class AlreadyActive() : DomainException.ValidationException(
            code: "Product.AlreadyActive",
            description: "Product has already been activated."
        );

        public sealed class AlreadyInactivate() : DomainException.ValidationException(
            code: "Product.AlreadyInactive",
            description: "Product has already been inactivated."
        );

        public sealed class SlugExists() : DomainException.ValidationException(
            code: "Product.SlugExists",
            description: "Please enter another name. Current name is already exist."
        );

        public sealed class InvalidPrice() : DomainException.ValidationException(
            code: "Product.InvalidPrice",
            description: "Product's price is invalid."
        );
    }
}
