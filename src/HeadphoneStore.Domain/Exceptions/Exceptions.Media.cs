namespace HeadphoneStore.Domain.Exceptions;

public static partial class Exceptions
{
    public static class Media
    {
        public sealed class UploadFailed() : DomainException.FailureException(
            code: "ProductMedia.UploadFailed",
            description: "Product's media cannot be found."
        );
    }
}
