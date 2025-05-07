using System.ComponentModel.DataAnnotations;

using HeadphoneStore.Domain.Enumerations;

namespace HeadphoneStore.Domain.Abstracts.Entities;

public abstract class Entity<T> : IEntity<T>, IAuditableEntity where T : notnull
{
    [Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Equivalents to [Key] (generate primary key) but have auto-increment.
    public T Id { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.Active;

    public DateTimeOffset CreatedDateTime { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedDateTime { get; protected set; }

    protected Entity()
    {
        CreatedDateTime = DateTimeOffset.UtcNow;
    }

    protected Entity(T Id)
    {
        this.Id = Id ?? throw new ArgumentNullException(nameof(Id));

        CreatedDateTime = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// In DDD, compare Entity actually using "Id" instead of reference.
    /// So by override the Equals, we can actually compare it by "Id".
    /// https://claude.ai/share/a0bffa44-eb58-4eea-8e34-2e6e62cc6854
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Entity<T>)obj;

        return EqualityComparer<T>.Default.Equals(Id, other.Id);
    }

    /// <summary>
    /// In general, when override Equals we have to override GetHashCode also.
    /// The GetHashCode means that usually when we call GetHashCode or any function that call GetHashCode,
    /// it will return the "hash code" of that object. But here we only want the Id comparison so that if it have the same "Id" it will return the same "hash code".
    /// </summary>
    public override int GetHashCode()
    {
        return Id?.GetHashCode() ?? 0;
    }

    public void Delete(string? updatedBy)
    {
        IsDeleted = true;
        Status = EntityStatus.Inactive;
        UpdatedDateTime = DateTimeOffset.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        Status = EntityStatus.Inactive;
    }

    protected void UpdateAudit(string? updatedBy)
    {
        UpdatedDateTime = DateTimeOffset.UtcNow;
    }

    public void Activate() => Status = EntityStatus.Active;

    public void Inactivate() => Status = EntityStatus.Inactive;
}
