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

        public sealed class InvalidPrice() : DomainException.ValidationException(
            code: "Product.InvalidPrice",
            description: "Product's price is invalid."
        );
    }
}
