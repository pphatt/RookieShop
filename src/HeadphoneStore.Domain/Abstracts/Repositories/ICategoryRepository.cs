using HeadphoneStore.Domain.Aggregates.Categories.Entities;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category, Guid>
{
}
