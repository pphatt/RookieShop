using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Aggregates.Order.Entities;

public class OrderDetail : Entity<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int ProductQuantity { get; private set; }
    public decimal ProductPrice { get; private set; }
    public decimal? ProductPriceDiscount { get; private set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    private OrderDetail() { } // For EF Core

    public OrderDetail(Guid orderId, Guid productId, int quantity, decimal price) : base(Guid.NewGuid())
    {
        OrderId = orderId;
        ProductId = productId;
        ProductQuantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be positive.");
        ProductPrice = price >= 0 ? price : throw new ArgumentException("Price cannot be negative.");
        CreatedDateTime  = DateTime.UtcNow;
    }
}
