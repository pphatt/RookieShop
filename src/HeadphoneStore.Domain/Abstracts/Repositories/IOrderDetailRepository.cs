using HeadphoneStore.Domain.Aggregates.Order.Entities;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IOrderDetailRepository : IRepositoryBase<OrderDetail, Guid>
{
}
