using HeadphoneStore.Domain.Abstracts.Entities;

namespace HeadphoneStore.Domain.Aggregates.Order.Entities;

public class OrderPayment : Entity<Guid>
{
    public string CardNumber { get; private set; }
    public string Cvc { get; private set; }
    public string Expire { get; private set; }

    private OrderPayment() { }
    public OrderPayment(string cardNumber, string cvc, string expire) : base(Guid.NewGuid())
    {
        CardNumber = cardNumber ?? throw new ArgumentNullException(nameof(cardNumber));
        Cvc = cvc ?? throw new ArgumentNullException(nameof(cvc));
        Expire = expire ?? throw new ArgumentNullException(nameof(expire));
        CreatedDateTime = DateTime.UtcNow;
    }
}
