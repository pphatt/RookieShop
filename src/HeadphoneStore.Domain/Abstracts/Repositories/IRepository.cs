using System.Linq.Expressions;

namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IRepositoryBase<TEntity, in TKey> where TEntity : class
{
    Task<TEntity> FindByIdAsync(
        TKey id,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties);

    IEnumerable<TEntity> FindByCondition(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties);

    IQueryable<TEntity> FindAll(
        Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties);

    IQueryable<TEntity> GetQueryableSet();

    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    Task RemoveAsync(TEntity entity);

    Task RemoveRangeAsync(IEnumerable<TEntity> entities);
}
