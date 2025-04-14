namespace HeadphoneStore.Domain.Abstracts.Entities;

public interface ICreatedByEntity<T>
{
    T CreatedBy { get; set; }
}
