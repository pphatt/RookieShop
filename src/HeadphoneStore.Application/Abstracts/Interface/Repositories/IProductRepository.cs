using HeadphoneStore.Domain.Aggregates.Products.Entities;

namespace HeadphoneStore.Application.Abstracts.Interface.Repositories;

public interface IProductRepository : IRepositoryBase<Product, Guid>
{
}
