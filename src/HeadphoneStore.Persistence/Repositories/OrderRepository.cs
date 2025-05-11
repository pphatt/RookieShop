using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Order.Entities;
using HeadphoneStore.Persistence.Repository;
using HeadphoneStore.Shared.Abstracts.Shared;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Persistence.Repositories;

public class OrderRepository : RepositoryBase<Order, Guid>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Order> GetOrderById(Guid id)
    {
        var order = await GetQueryableSet()
            .Where(x => x.Id == id)
            .Include(x => x.Customer)
            .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync();

        return order;
    }

    public async Task<PagedResult<Order>> GetAllOrdersPagination(Guid? userId, string? searchTerm, int pageIndex, int pageSize)
    {
        var query = GetQueryableSet()
            .Include(x => x.Customer)
            .Include(x => x.OrderDetails).ThenInclude(x => x.Product)
            .AsNoTracking()
            .AsQueryable()
            .IgnoreQueryFilters();

        if (userId is not null)
        {
            query = query.Where(x => x.CustomerId == userId);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => x.OrderDetails.Any(x => x.Product.Name.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant())));
        }

        query = query
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedDateTime);

        return await PagedResult<Order>.InitializeAsync(query, pageIndex, pageSize);
    }
}
