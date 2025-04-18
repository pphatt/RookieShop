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

    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);
}
