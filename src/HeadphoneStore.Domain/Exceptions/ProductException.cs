namespace HeadphoneStore.Domain.Exceptions;

public static class ProductException
{
    public sealed class ProductNotFoundException() : DomainException.NotFoundException("Product.NotFound", "Product cannot be found");
    public sealed class ProductNameDuplicateException() : DomainException.ValidationException("Product.NameDuplicate", "Product already exists");
}
