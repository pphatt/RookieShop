using System.Diagnostics;

using HeadphoneStore.Domain.Abstracts.ValueObjects;

namespace HeadphoneStore.Domain.Aggregates.Products.ValueObjects;

public class ProductPrice : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    protected ProductPrice() { }
    public ProductPrice(decimal amount, string currency) : this()
    {
        if (amount < 0) throw new ArgumentException("Amount must be positive.");

        Amount = amount;
        Currency = currency;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        return [Amount, Currency];
    }

    public static ProductPrice Create(decimal amount, string currency)
        => new(amount, currency);

    public static ProductPrice CreateEmpty(string currency)
        => new(0, currency);
}
