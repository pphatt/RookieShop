using HeadphoneStore.Shared.Dtos.Identity.User;
using HeadphoneStore.Shared.Services.Order.CreateOrder;

namespace HeadphoneStore.Shared.Dtos.Order;

public class OrderDto
{
    public Guid Id { get; set; }
    public string Note { get; set; }
    public string PhoneNumber { get; set; }
    public string Status { get; set; }
    public bool IsFeedback { get; set; }
    public decimal TotalPrice { get; set; }
    public string PaymentMethod { get; set; }
    public ShippingAddressDto ShippingAddress { get; set; }
    public UserDto Customer { get; set; }
    public List<OrderDetailDto> OrderDetails { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }
    public DateTimeOffset? ModifiedDateTime { get; set; }
}
