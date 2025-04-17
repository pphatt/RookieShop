using System.Linq.Expressions;

using HeadphoneStore.Domain.Abstracts.Entities;
using HeadphoneStore.Domain.Abstracts.Repositories;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Persistence.Repository;

public class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey>
    where TEntity : Entity<TKey>
{
    private readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(ApplicationDbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity> FindByIdAsync(
        TKey id,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await FindAllAsync(null, includeProperties)
            .AsTracking()
            .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public IEnumerable<TEntity> FindByConditionAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return FindAllAsync(predicate, includeProperties).AsTracking();
    }

    public IQueryable<TEntity> FindAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> items = _dbSet.AsNoTracking(); // Keep AsNoTracking for query side

        if (predicate != null)
        {
            items = items.Where(predicate);
        }

        if (includeProperties != null)
        {
            foreach (var property in includeProperties)
            {
                items = items.Include(property);
            }
        }

        return items;
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}
