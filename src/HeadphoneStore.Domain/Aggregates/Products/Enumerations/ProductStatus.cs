using System.Text.Json.Serialization;

namespace HeadphoneStore.Domain.Aggregates.Products.Enumerations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProductStatus : byte
{
    InStock = 1,
    OutOfStock = 2,
    Discontinued = 3
}
