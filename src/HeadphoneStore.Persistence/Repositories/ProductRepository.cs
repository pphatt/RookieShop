using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Persistence.Repository;

namespace HeadphoneStore.Persistence.Repositories;

public class ProductRepository : RepositoryBase<Product, Guid>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }
}
