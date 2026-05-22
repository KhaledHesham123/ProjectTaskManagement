using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity>(AppDbContext dbContext) : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await _dbSet.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) =>
        await _dbSet.AddRangeAsync(entities, cancellationToken);

    public void Update(TEntity entity) => _dbSet.Update(entity);

    public void SaveInclude(TEntity entity, params string[] includedProperties)
    {
        var localEntity = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);
        EntityEntry<TEntity> entry;

        if (localEntity is not null)
        {
            entry = dbContext.Entry(localEntity);

            foreach (var propertyName in includedProperties)
            {
                var newValue = dbContext.Entry(entity).Property(propertyName).CurrentValue;
                entry.Property(propertyName).CurrentValue = newValue;
                entry.Property(propertyName).IsModified = true;
            }
        }
        else
        {
            _dbSet.Attach(entity);
            entry = dbContext.Entry(entity);

            foreach (var propertyName in includedProperties)
                entry.Property(propertyName).IsModified = true;
        }
    }

    public void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
    }

    public IQueryable<TEntity> GetAll() =>
        _dbSet.Where(x => !x.IsDeleted);

    public IQueryable<TEntity> GetByCriteriaQueryable(Expression<Func<TEntity, bool>> predicate) =>
        _dbSet.Where(x => !x.IsDeleted).Where(predicate);

    public Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => !x.IsDeleted);

        if (predicate is not null)
            query = query.Where(predicate);

        return query.CountAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => !x.IsDeleted);

        if (predicate is not null)
            query = query.Where(predicate);

        return query.AnyAsync(cancellationToken);
    }
}
