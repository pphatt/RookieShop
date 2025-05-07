using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Order.Entities;
using HeadphoneStore.Persistence.Repository;

namespace HeadphoneStore.Persistence.Repositories;

public class OrderRepository : RepositoryBase<Order, Guid>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }
}
