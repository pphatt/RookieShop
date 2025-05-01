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

        public sealed class AlreadyDeleted() : DomainException.ValidationException(
            code: "Brand.AlreadyDeleted",
            description: "Brand has already been deleted."
        );

        public sealed class AlreadyActive() : DomainException.ValidationException(
            code: "Brand.AlreadyActive",
            description: "Brand has already been activated."
        );

        public sealed class AlreadyInactivate() : DomainException.ValidationException(
            code: "Brand.AlreadyInactive",
            description: "Brand has already been inactivated."
        );

        public sealed class SlugExists() : DomainException.ValidationException(
            code: "Brand.SlugExists",
            description: "Please enter another name. Current name is already exist."
        );
    }
}
