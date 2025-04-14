using HeadphoneStore.Domain.Abstracts.ValueObjects;
using HeadphoneStore.Domain.Constraints;

namespace HeadphoneStore.Domain.Aggregates.Order.ValueObjects;

public class ShippingAddress : ValueObject
{
    public string Street { get; }
    public string Province { get; }
    public string PhoneNumber { get; }

    protected ShippingAddress() { }

    public ShippingAddress(string street, string province, string phoneNumber)
    {
        //if (!string.IsNullOrWhiteSpace(street) && street.Length > DataLength.Medium)
        //    return Result<ShippingAddress>.Invalid(
        //        new ValidationError($"Street must be less than {DataLength.Medium} characters"));

        //if (!string.IsNullOrWhiteSpace(city) && city.Length > DataLength.Medium)
        //    return Result<ShippingAddress>.Invalid(
        //        new ValidationError($"City must be less than {DataLength.Medium} characters"));

        //if (!string.IsNullOrWhiteSpace(province) && province.Length > DataLength.Medium)
        //    return Result<ShippingAddress>.Invalid(
        //        new ValidationError($"Province must be less than {DataLength.Medium} characters"));

        Street = street;
        Province = province;
        PhoneNumber = phoneNumber;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return Province;
        yield return PhoneNumber;
    }
}
