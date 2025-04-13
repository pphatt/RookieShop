using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Abstracts.Aggregates;

public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot where T : notnull
{
    protected AggregateRoot() : base() { }
    protected AggregateRoot(T id) : base(id) { }
    protected virtual void EnsureValidState() { }
}
