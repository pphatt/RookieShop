using HeadphoneStore.Shared.Dtos.Order;

namespace HeadphoneStore.Shared.Services.Order.CreateOrder;

public class ShippingAddressDto
{
    public string StreetAddress { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string CityProvince { get; set; } = string.Empty;
}

public class CreateOrderRequestDto
{
    public string Email { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string CustomerPhoneNumber { get; set; } = "";
    public string PaymentMethod { get; set; }
    public ShippingAddressDto ShippingAddress { get; set; } = new ShippingAddressDto();
    public string Note { get; set; } = "";
    public List<CreateOrderItemDto> OrderItems { get; set; } = [];
}
