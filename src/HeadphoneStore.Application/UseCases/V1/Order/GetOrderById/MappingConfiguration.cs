using HeadphoneStore.Shared.Services.Order.GetOrderById;

namespace HeadphoneStore.Application.UseCases.V1.Order.GetOrderById;

public static class MappingConfiguration
{
    public static GetOrderByIdQuery MapToQuery(this GetOrderByIdRequestDto dto, Guid customerId)
        => new(dto.Id, customerId);
}
