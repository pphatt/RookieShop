namespace HeadphoneStore.Domain.Abstracts.Entities;

public interface IAuditableEntity
{
    DateTimeOffset CreatedDateTime { get; }
    DateTimeOffset? ModifiedDateTime { get; }
}
