namespace HeadphoneStore.Domain.Abstracts.Entities;

public interface IUpdatedByEntity<T>
{
    T? UpdatedBy { get; set; }
}
