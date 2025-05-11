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
        return await FindAll(null, includeProperties)
            .AsTracking()
            .SingleOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken) ?? null!;
    }

    public IEnumerable<TEntity> FindByCondition(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return FindAll(predicate, includeProperties).AsTracking();
    }

    public IQueryable<TEntity> FindAll(
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

    public IQueryable<TEntity> GetQueryableSet()
    {
        return _dbSet.AsQueryable<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var entry = _dbSet.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            _dbSet.Attach(entity);

            entry.State = EntityState.Modified;
        }

        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        _dbSet.UpdateRange(entities);

        return Task.CompletedTask;
    }

    public Task RemoveAsync(TEntity entity)
    {
        _dbSet.Remove(entity);

        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);

        return Task.CompletedTask;
    }
}
