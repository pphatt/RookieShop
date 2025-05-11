using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Order;

namespace HeadphoneStore.Application.UseCases.V1.Order.GetOrderById;

public class GetOrderByIdQuery : IQuery<OrderDto>
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
}
