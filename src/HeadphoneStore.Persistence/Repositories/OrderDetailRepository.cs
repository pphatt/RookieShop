using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Order.Entities;
using HeadphoneStore.Persistence.Repository;

namespace HeadphoneStore.Persistence.Repositories;

public class OrderDetailRepository : RepositoryBase<OrderDetail, Guid>, IOrderDetailRepository
{
    public OrderDetailRepository(ApplicationDbContext context) : base(context)
    {
    }
}
