using HeadphoneStore.Application.Abstracts.Interface.Repositories;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Persistence.Repository;

namespace HeadphoneStore.Persistence.Repositories;

public class ProductMediaRepository : RepositoryBase<ProductMedia, Guid>, IProductMediaRepository
{
    public ProductMediaRepository(ApplicationDbContext context) : base(context)
    {
    }
}
