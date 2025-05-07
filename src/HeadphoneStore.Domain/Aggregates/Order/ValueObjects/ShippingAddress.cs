using System.IO;

using HeadphoneStore.Domain.Abstracts.ValueObjects;

namespace HeadphoneStore.Domain.Aggregates.Order.ValueObjects;

public class ShippingAddress : ValueObject
{
    public string StreetAddress { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string CityProvince { get; set; } = string.Empty;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StreetAddress;
        yield return Ward;
        yield return District;
        yield return CityProvince;
    }
}
