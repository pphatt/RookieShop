using HeadphoneStore.Domain.Aggregates.Order.ValueObjects;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Services.Order.CreateOrder;

namespace HeadphoneStore.Application.UseCases.V1.Order.CreateOrder;

public static class MappingConfiguration
{
    public static CreateOrderCommand MapToCommand(this CreateOrderRequestDto dto, Guid customerId)
    {
        Enum.TryParse<PaymentMethod>(dto.PaymentMethod, false, out var paymentMethod);

        var shippingAddress = new ShippingAddress
        {
            StreetAddress = dto.ShippingAddress.StreetAddress,
            Ward = dto.ShippingAddress.Ward,
            District = dto.ShippingAddress.District,
            CityProvince = dto.ShippingAddress.CityProvince
        };

        return new(customerId,
               dto.Email,
               dto.CustomerName,
               dto.CustomerPhoneNumber,
               paymentMethod,
               shippingAddress,
               dto.Note,
               dto.OrderItems);
    }
}
