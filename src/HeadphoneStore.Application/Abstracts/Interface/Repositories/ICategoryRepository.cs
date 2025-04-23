using HeadphoneStore.Domain.Aggregates.Categories.Entities;

namespace HeadphoneStore.Application.Abstracts.Interface.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category, Guid>
{
}
