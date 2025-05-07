namespace HeadphoneStore.Domain.Exceptions;

public static partial class Exceptions
{
    public static class Order
    {
        public sealed class NotFound() : DomainException.NotFoundException(
            code: "Order.NotFound",
            description: "Order was not found."
        );

        public sealed class AlreadyDeleted() : DomainException.ValidationException(
            code: "Order.AlreadyDeleted",
            description: "Order has already been deleted."
        );
    }
}
