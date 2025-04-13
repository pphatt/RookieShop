using HeadphoneStore.Domain.Abstracts.Aggregates;
using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Entities.Content;

using HeadphoneStore.Domain.Abstracts.ValueObjects;

public class OrderAddress : ValueObject
{
    public string Address { get; }
    public string Street { get; }
    public string Province { get; }
    public string PhoneNumber { get; }

    public OrderAddress(string address, string street, string province, string phoneNumber)
    {
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Street = street ?? throw new ArgumentNullException(nameof(street));
        Province = province ?? throw new ArgumentNullException(nameof(province));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return Province;
        yield return PhoneNumber;
    }
}

public class Order : AggregateRoot<Guid>, ICreatedByEntity<Guid>, IUpdatedByEntity<Guid?>
{
    public Guid UserId { get; private set; }
    public OrderAddress ShippingAddress { get; private set; }
    public string Note { get; private set; }
    public string Status { get; private set; }
    public bool IsFeedback { get; private set; }
    public decimal Total { get; private set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    private readonly List<OrderDetail> _orderDetails = new();
    public virtual IReadOnlyCollection<OrderDetail> OrderDetails => _orderDetails.AsReadOnly();

    private readonly List<OrderPayment> _payments = new();
    public virtual IReadOnlyCollection<OrderPayment> Payments => _payments.AsReadOnly();

    private Order() { } // For EF Core

    public Order(Guid userId, OrderAddress shippingAddress, string note, Guid createdBy) : base(Guid.NewGuid())
    {
        UserId = userId;
        ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
        Note = note ?? string.Empty;
        Status = "Pending";
        IsFeedback = false;
        Total = 0;
        CreatedOnUtc = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void AddOrderDetail(Guid orderId, Guid productId, int quantity, decimal price)
    {
        if (Status != "Pending")
            throw new InvalidOperationException("Cannot add items to a non-pending order.");

        var detail = new OrderDetail(orderId, productId, quantity, price);
        _orderDetails.Add(detail);
        RecalculateTotal();
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public void AddPayment(string cardNumber, string cvc, string expire)
    {
        if (Status != "Pending")
            throw new InvalidOperationException("Cannot add payments to a non-pending order.");

        var payment = new OrderPayment(cardNumber, cvc, expire);
        _payments.Add(payment);
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public void UpdateStatus(string newStatus, Guid updatedBy)
    {
        var validStatuses = new[] { "Pending", "Confirmed", "Shipped", "Delivered", "Cancelled" };
        if (!validStatuses.Contains(newStatus))
            throw new ArgumentException("Invalid order status.");

        if (Status == "Delivered" && newStatus != "Delivered")
            throw new InvalidOperationException("Cannot revert a delivered order.");

        Status = newStatus;
        UpdatedBy = updatedBy;
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public void MarkFeedbackProvided(Guid updatedBy)
    {
        if (Status != "Delivered")
            throw new InvalidOperationException("Feedback can only be provided for delivered orders.");

        IsFeedback = true;
        UpdatedBy = updatedBy;
        ModifiedOnUtc = DateTime.UtcNow;
    }

    private void RecalculateTotal()
    {
        Total = _orderDetails.Sum(d => d.ProductQuantity * (d.ProductPriceDiscount ?? d.ProductPrice));
    }

    protected override void EnsureValidState()
    {
        if (!_orderDetails.Any() && Status != "Pending")
            throw new InvalidOperationException("Non-pending order must have at least one item.");

        if (_payments.Any() && Status == "Cancelled")
            throw new InvalidOperationException("Cancelled order cannot have payments.");
    }
}
