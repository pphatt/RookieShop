namespace HeadphoneStore.Domain.Exceptions;

public static partial class Exceptions
{
    public static class Brand
    {
        public sealed class NotFound() : DomainException.ValidationException(
            code: "Brand.NotFound",
            description: "Brand cannot be found."
        );
    }
}
