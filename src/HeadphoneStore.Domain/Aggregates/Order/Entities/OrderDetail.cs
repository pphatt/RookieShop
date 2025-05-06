using HeadphoneStore.Domain.Abstracts.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Entities;

namespace HeadphoneStore.Domain.Aggregates.Order.Entities;

public class OrderDetail : Entity<Guid>
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice => Quantity * Price;

    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; }

    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }

    private OrderDetail() { } // For EF Core

    public OrderDetail(Guid orderId, Guid productId, int quantity, decimal price) : base(Guid.NewGuid())
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be positive.");
        Price = price >= 0 ? price : throw new ArgumentException("Price cannot be negative.");
        CreatedDateTime = DateTime.UtcNow;
    }
}
