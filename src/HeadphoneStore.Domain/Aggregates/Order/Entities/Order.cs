using HeadphoneStore.Domain.Abstracts.Aggregates;

namespace HeadphoneStore.Domain.Aggregates.Order.Entities;

using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Aggregates.Order.Enumerations;
using HeadphoneStore.Domain.Aggregates.Order.ValueObjects;
using HeadphoneStore.Domain.Enumerations;

public class Order : AggregateRoot<Guid>
{
    public string Note { get; private set; }
    public string PhoneNumber { get; set; }
    public OrderStatus Status { get; private set; }
    public bool IsFeedback { get; private set; }
    public decimal TotalPrice { get; private set; }

    // temporary
    public PaymentMethod PaymentMethod { get; set; }

    public ShippingAddress ShippingAddress { get; private set; }

    public Guid CustomerId { get; private set; }
    public virtual AppUser Customer { get; set; }

    private readonly List<OrderDetail> _orderDetails = new();
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    private readonly List<OrderPayment> _payments = new();
    public virtual IReadOnlyCollection<OrderPayment> Payments => _payments.AsReadOnly();

    private static readonly TimeZoneInfo VietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

    private Order() { } // For EF Core

    public Order(Guid userId, ShippingAddress shippingAddress, string note) : base(Guid.NewGuid())
    {
        CustomerId = userId;
        ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
        Note = note ?? string.Empty;
        Status = OrderStatus.Pending;
        IsFeedback = false;
        CreatedDateTime = DateTimeOffset.Now;
    }

    public static Order Create(Guid customerId, string note, string phoneNumber, decimal totalPrice, PaymentMethod paymentMethod, ShippingAddress shippingAddress)
    {
        return new Order
        {
            CustomerId = customerId,
            Note = note,
            PhoneNumber = phoneNumber,
            TotalPrice = totalPrice,
            PaymentMethod = paymentMethod,
            ShippingAddress = shippingAddress,
            IsFeedback = false,
            Status = OrderStatus.Pending,
            CreatedDateTime = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, VietnamTimeZone)
        };
    }

    public void AddOrderDetail(Guid orderId, Guid productId, int quantity, decimal price)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot add items to a non-pending order.");

        var detail = new OrderDetail(orderId, productId, quantity, price);
        _orderDetails.Add(detail);
        RecalculateTotal();
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void AddPayment(string cardNumber, string cvc, string expire)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot add payments to a non-pending order.");

        var payment = new OrderPayment(cardNumber, cvc, expire);
        _payments.Add(payment);
        UpdatedDateTime = DateTime.Now;
    }

    public void UpdateStatus(string newStatus, Guid updatedBy)
    {
        var validStatuses = new[] { "Pending", "Ordered", "Cancelled" };

        if (!validStatuses.Contains(newStatus))
            throw new ArgumentException("Invalid order status.");

        if (Status == OrderStatus.Ordered && newStatus != OrderStatus.Ordered.ToString())
            throw new InvalidOperationException("Cannot revert a delivered order.");

        Status = Enum.Parse<OrderStatus>(newStatus);
        UpdatedDateTime = DateTime.Now;
    }

    public void MarkFeedbackProvided(Guid updatedBy)
    {
        if (Status != OrderStatus.Ordered)
            throw new InvalidOperationException("Feedback can only be provided for delivered orders.");

        IsFeedback = true;
        UpdatedDateTime = DateTime.Now;
    }

    private void RecalculateTotal()
    {
        TotalPrice = _orderDetails.Sum(d => d.Quantity * d.Price);
    }

    protected override void EnsureValidState()
    {
        if (!_orderDetails.Any() && Status != OrderStatus.Pending)
            throw new InvalidOperationException("Non-pending order must have at least one item.");

        if (_payments.Any() && Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Cancelled order cannot have payments.");
    }
}
