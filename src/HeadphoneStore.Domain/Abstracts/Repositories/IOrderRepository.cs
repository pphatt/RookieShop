using HeadphoneStore.Domain.Aggregates.Order.Entities;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Order;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IOrderRepository : IRepositoryBase<Order, Guid>
{
    Task<Order> GetOrderById(Guid id);
    Task<PagedResult<Order>> GetAllOrdersPagination(Guid? userId, string? searchTerm, int pageIndex, int pageSize);
}
