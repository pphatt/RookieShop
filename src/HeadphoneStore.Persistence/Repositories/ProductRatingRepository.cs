using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Persistence.Repository;

namespace HeadphoneStore.Persistence.Repositories;

public class ProductRatingRepository : RepositoryBase<ProductRating, Guid>, IProductRatingRepository
{
    public ProductRatingRepository(ApplicationDbContext context) : base(context)
    {
    }
}
