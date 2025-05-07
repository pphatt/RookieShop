using HeadphoneStore.Domain.Aggregates.Order.ValueObjects;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Dtos.Order;

namespace HeadphoneStore.Application.UseCases.V1.Order.CreateOrder;

public class CreateOrderCommand : ICommand
{
    public Guid CustomerId { get; set; }
    public string Email { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string CustomerPhoneNumber { get; set; } = "";
    public PaymentMethod PaymentMethod { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
    public string Note { get; set; } = "";
    public List<CreateOrderItemDto> OrderItems { get; set; } = [];
}
