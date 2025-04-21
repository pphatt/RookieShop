namespace HeadphoneStore.Domain.Exceptions;

public static partial class Exceptions
{
    public static class Brand
    {
        public sealed class NotFound() : DomainException.NotFoundException(
            code: "Brand.NotFound",
            description: "Brand cannot be found."
        );

        public sealed class DuplicateName() : DomainException.ValidationException(
            code: "Brand.DuplicateName",
            description: "Brand's name already exists."
        );
    }
}
