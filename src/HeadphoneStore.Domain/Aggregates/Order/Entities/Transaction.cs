using HeadphoneStore.Domain.Abstracts.Aggregates;
using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Aggregates.Order.Entities;

public class Transaction : AggregateRoot<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public decimal Total { get; private set; }
    public string Note { get; private set; }
    public Guid UserId { get; private set; }
    public string Status { get; private set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    private Transaction() { } // For EF Core

    public Transaction(decimal total, string note, Guid userId, Guid createdBy) : base(Guid.NewGuid())
    {
        Total = total >= 0 ? total : throw new ArgumentException("Total cannot be negative.");
        Note = note ?? throw new ArgumentNullException(nameof(note));
        UserId = userId;
        Status = "Pending";
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateStatus(string newStatus, Guid updatedBy)
    {
        var validStatuses = new[] { "Pending", "Completed", "Failed" };
        if (!validStatuses.Contains(newStatus))
            throw new ArgumentException("Invalid transaction status.");

        Status = newStatus;
        UpdatedBy = updatedBy;
        ModifiedOnUtc = DateTime.UtcNow;
    }
}
