using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Persistence.Repository;

namespace HeadphoneStore.Persistence.Repositories;

public class BrandRepository : RepositoryBase<Brand, Guid>, IBrandRepository
{
    public BrandRepository(ApplicationDbContext context) : base(context)
    {
    }
}
