using HeadphoneStore.Domain.Abstracts.Aggregates;

namespace HeadphoneStore.Domain.Aggregates.Order.Entities;

using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Aggregates.Order.ValueObjects;

public class Order : AggregateRoot<Guid>
{
    public string Note { get; private set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Status { get; private set; }
    public bool IsFeedback { get; private set; }
    public decimal TotalPrice { get; private set; }

    public ShippingAddress ShippingAddress { get; private set; }

    public Guid UserId { get; private set; }
    public virtual AppUser User { get; set; }

    private readonly List<OrderDetail> _orderDetails = new();
    public virtual IReadOnlyCollection<OrderDetail> OrderDetails => _orderDetails.AsReadOnly();

    private readonly List<OrderPayment> _payments = new();
    public virtual IReadOnlyCollection<OrderPayment> Payments => _payments.AsReadOnly();

    private Order() { } // For EF Core

    public Order(Guid userId, ShippingAddress shippingAddress, string note) : base(Guid.NewGuid())
    {
        UserId = userId;
        ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
        Note = note ?? string.Empty;
        Status = "Pending";
        IsFeedback = false;
        CreatedDateTime = DateTime.UtcNow;
    }

    public void AddOrderDetail(Guid orderId, Guid productId, int quantity, decimal price)
    {
        if (Status != "Pending")
            throw new InvalidOperationException("Cannot add items to a non-pending order.");

        var detail = new OrderDetail(orderId, productId, quantity, price);
        _orderDetails.Add(detail);
        RecalculateTotal();
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void AddPayment(string cardNumber, string cvc, string expire)
    {
        if (Status != "Pending")
            throw new InvalidOperationException("Cannot add payments to a non-pending order.");

        var payment = new OrderPayment(cardNumber, cvc, expire);
        _payments.Add(payment);
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void UpdateStatus(string newStatus, Guid updatedBy)
    {
        var validStatuses = new[] { "Pending", "Confirmed", "Shipped", "Delivered", "Cancelled" };
        if (!validStatuses.Contains(newStatus))
            throw new ArgumentException("Invalid order status.");

        if (Status == "Delivered" && newStatus != "Delivered")
            throw new InvalidOperationException("Cannot revert a delivered order.");

        Status = newStatus;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void MarkFeedbackProvided(Guid updatedBy)
    {
        if (Status != "Delivered")
            throw new InvalidOperationException("Feedback can only be provided for delivered orders.");

        IsFeedback = true;
        UpdatedDateTime = DateTime.UtcNow;
    }

    private void RecalculateTotal()
    {
        TotalPrice = _orderDetails.Sum(d => d.Quantity * d.Price);
    }

    protected override void EnsureValidState()
    {
        if (!_orderDetails.Any() && Status != "Pending")
            throw new InvalidOperationException("Non-pending order must have at least one item.");

        if (_payments.Any() && Status == "Cancelled")
            throw new InvalidOperationException("Cancelled order cannot have payments.");
    }
}
