using HeadphoneStore.Domain.Aggregates.Order.ValueObjects;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Dtos.Order;

namespace HeadphoneStore.Application.UseCases.V1.Order.CreateOrder;

public sealed record CreateOrderCommand(Guid CustomerId,
                                        string Email,
                                        string CustomerName,
                                        string CustomerPhoneNumber,
                                        PaymentMethod PaymentMethod,
                                        ShippingAddress ShippingAddress,
                                        string Note,
                                        List<CreateOrderItemDto> OrderItems) : ICommand
{
}
