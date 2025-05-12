using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Order;

namespace HeadphoneStore.Application.UseCases.V1.Order.GetOrderById;

public sealed record GetOrderByIdQuery(Guid Id, Guid CustomerId) : IQuery<OrderDto>
{
}
