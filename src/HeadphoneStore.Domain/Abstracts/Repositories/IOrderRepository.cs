using HeadphoneStore.Domain.Aggregates.Order.Entities;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IOrderRepository : IRepositoryBase<Order, Guid>
{
}
