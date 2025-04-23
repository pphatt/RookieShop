using HeadphoneStore.Domain.Aggregates.Products.Entities;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IProductRepository : IRepositoryBase<Product, Guid>
{
}
