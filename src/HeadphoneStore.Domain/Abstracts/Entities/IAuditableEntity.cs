namespace HeadphoneStore.Domain.Abstracts.Entities;

public interface IAuditableEntity
{
    DateTimeOffset CreatedOnUtc { get; }
    DateTimeOffset? ModifiedOnUtc { get; }
}
