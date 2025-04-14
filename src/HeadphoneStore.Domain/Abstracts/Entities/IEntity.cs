namespace HeadphoneStore.Domain.Abstracts.Entities;

public interface IEntity<T>
{
    T Id { get; }
    bool IsDeleted { get; }
}
