namespace HeadphoneStore.Domain.Abstracts.ValueObjects;

/// <summary>
/// Implementation of DDD value object from Vladimir Khorikov:
/// https://enterprisecraftsmanship.com/posts/value-object-better-implementation/
/// </summary>
public abstract class ValueObject
{
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        var valueObject = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return current * 23 + (obj?.GetHashCode() ?? 0);
                }
            });
    }

    /// <summary>
    /// This ensures that two Money objects with the same amount and currency are considered equal, regardless of whether they're the same instance in memory.
    /// https://claude.ai/share/a0bffa44-eb58-4eea-8e34-2e6e62cc6854
    /// </summary>
    protected abstract IEnumerable<object> GetEqualityComponents();

    public static bool operator ==(ValueObject a, ValueObject b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }
}
