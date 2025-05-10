using HeadphoneStore.Domain.Abstracts.Aggregates;

namespace HeadphoneStore.Domain.Aggregates.Order.Entities;

using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Aggregates.Order.Enumerations;
using HeadphoneStore.Domain.Aggregates.Order.ValueObjects;
using HeadphoneStore.Domain.Enumerations;

public class Order : AggregateRoot<Guid>
{
    public string Note { get; private set; }
    public string PhoneNumber { get; private set; }
    public OrderStatus Status { get; private set; }
    public bool IsFeedback { get; private set; }
    public decimal TotalPrice { get; private set; }

    // temporary
    public PaymentMethod PaymentMethod { get; private set; }

    public ShippingAddress ShippingAddress { get; private set; }

    public Guid CustomerId { get; private set; }
    public virtual AppUser Customer { get; private set; }

    private readonly List<OrderDetail> _orderDetails = new();
    public virtual ICollection<OrderDetail> OrderDetails => _orderDetails.AsReadOnly();

    private readonly List<OrderPayment> _payments = new();
    public virtual IReadOnlyCollection<OrderPayment> Payments => _payments.AsReadOnly();

    protected Order() { }
    protected Order(Guid customerId,
                    string note,
                    string phoneNumber,
                    PaymentMethod paymentMethod,
                    ShippingAddress shippingAddress) : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        Note = note;
        PhoneNumber = phoneNumber;
        PaymentMethod = paymentMethod;
        ShippingAddress = shippingAddress;
        IsFeedback = false;
        Status = OrderStatus.Pending;
        CreatedDateTime = DateTimeOffset.UtcNow;
    }

    public static Order Create(Guid customerId,
                               string note,
                               string phoneNumber,
                               PaymentMethod paymentMethod,
                               ShippingAddress shippingAddress)
        => new(customerId, note, phoneNumber, paymentMethod, shippingAddress);

    public void CreateOrderDetail(Guid productId, int quantity, decimal price)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot add items to a non-pending order.");

        var detail = OrderDetail.Create(productId, quantity, price);

        _orderDetails.Add(detail);

        TotalPrice = _orderDetails.Sum(d => d.Quantity * d.Price);

        ModifiedDateTime = DateTime.UtcNow;
    }

    public void AddPayment(string cardNumber, string cvc, string expire)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot add payments to a non-pending order.");

        var payment = new OrderPayment(cardNumber, cvc, expire);

        _payments.Add(payment);

        ModifiedDateTime = DateTime.Now;
    }

    public void UpdateStatus(string newStatus, Guid updatedBy)
    {
        var validStatuses = new[] { "Pending", "Ordered", "Cancelled" };

        if (!validStatuses.Contains(newStatus))
            throw new ArgumentException("Invalid order status.");

        if (Status == OrderStatus.Ordered && newStatus != OrderStatus.Ordered.ToString())
            throw new InvalidOperationException("Cannot revert a delivered order.");

        Status = Enum.Parse<OrderStatus>(newStatus);
        ModifiedDateTime = DateTime.Now;
    }

    public void MarkFeedbackProvided(Guid updatedBy)
    {
        if (Status != OrderStatus.Ordered)
            throw new InvalidOperationException("Feedback can only be provided for delivered orders.");

        IsFeedback = true;
        ModifiedDateTime = DateTime.Now;
    }

    protected override void EnsureValidState()
    {
        if (!_orderDetails.Any() && Status != OrderStatus.Pending)
            throw new InvalidOperationException("Non-pending order must have at least one item.");

        if (_payments.Any() && Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Cancelled order cannot have payments.");
    }
}
