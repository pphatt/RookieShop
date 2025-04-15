namespace HeadphoneStore.Domain.Exceptions;

public static class ProductException
{
    public sealed class ProductNotFoundException() : DomainException.NotFoundException(
        code: "Product.NotFound", 
        description: "Product cannot be found"
    );

    public sealed class ProductNameDuplicateException() : DomainException.ValidationException(
        code: "Product.NameDuplicate", 
        description: "Product already exists"
    );
}
