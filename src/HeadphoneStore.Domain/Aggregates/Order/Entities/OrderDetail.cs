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

    protected OrderDetail() { }
    protected OrderDetail(Guid productId, int quantity, decimal price) : base(Guid.NewGuid())
    {
        ProductId = productId;
        Quantity = quantity;
        Price = price;
        CreatedDateTime = DateTime.UtcNow;
    }

    public static OrderDetail Create(Guid productId, int quantity, decimal price)
        => new(productId, quantity, price);
}
