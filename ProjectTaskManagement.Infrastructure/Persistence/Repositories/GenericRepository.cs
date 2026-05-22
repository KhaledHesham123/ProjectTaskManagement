using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.Helper.Extensions;
using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity>(AppDbContext dbContext) : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public void Remove(TEntity entity) => _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

    public void Update(TEntity entity) => _dbSet.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

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

    public void Delete(TEntity entity) => entity.IsDeleted = true;

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await _dbSet.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) =>
        await _dbSet.AddRangeAsync(entities, cancellationToken);

    public IQueryable<TEntity> GetAll() =>
        dbContext.Set<TEntity>().Where(x => !x.IsDeleted).AsNoTracking();

    public IQueryable<TEntity> GetByCriteriaQueryable(Expression<Func<TEntity, bool>> predicate) =>
        dbContext.Set<TEntity>()
            .Where(x => !x.IsDeleted)
            .Where(predicate)
            .AsNoTracking();

    public Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> filterPredicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Expression<Func<TEntity, TEntity>>? select = null,
        bool asNoTracking = false,
        bool asSplit = false,
        bool ignoreFilter = false,
        bool withDeleted = false,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        query = query.Where(filterPredicate);

        if (!withDeleted)
            query = query.Where(a => !a.IsDeleted);
        if (ignoreFilter)
            query = query.IgnoreQueryFilters();
        if (include is not null)
            query = include(query);
        if (asNoTracking)
            query = query.AsNoTracking();
        if (select is not null)
            query = query.Select(select);
        if (asSplit)
            query = query.AsSplitQuery();
        if (orderBy is not null)
            query = orderBy(query);

        return query.FirstOrDefaultAsync(cancellationToken)!;
    }

    public Task<TResult?> FindAsync<TResult>(
        Expression<Func<TEntity, bool>> filterPredicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool asNoTracking = false,
        Expression<Func<TEntity, TResult>>? select = null,
        bool asSplit = false,
        bool ignoreFilter = false,
        bool withDeleted = false,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (!withDeleted)
            query = query.Where(a => !a.IsDeleted);

        query = query.Where(filterPredicate);

        if (ignoreFilter)
            query = query.IgnoreQueryFilters();
        if (include is not null)
            query = include(query);
        if (asNoTracking)
            query = query.AsNoTracking();
        if (asSplit)
            query = query.AsSplitQuery();

        if (select is not null)
            return query.Select(select).FirstOrDefaultAsync(cancellationToken)!;

        return query.Cast<TResult>().FirstOrDefaultAsync(cancellationToken)!;
    }

    public Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> filterPredicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Expression<Func<TEntity, TEntity>>? select = null,
        int? take = null,
        bool ignoreFilter = false,
        bool asSplit = false,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        query = query.Where(a => !a.IsDeleted).Where(filterPredicate);

        if (ignoreFilter)
            query = query.IgnoreQueryFilters();
        if (include is not null)
            query = include(query);
        if (orderBy is not null)
            query = orderBy(query);
        if (take is not null)
            query = query.Take(take.Value);
        if (select is not null)
            query = query.Select(select);
        if (asSplit)
            query = query.AsSplitQuery();

        return query.AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken);
    }

    public Task<List<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, bool>> filterPredicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Expression<Func<TEntity, TResult>>? select = null,
        int? take = null,
        bool ignoreFilter = false,
        bool asSplit = false,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        query = query.Where(a => !a.IsDeleted).Where(filterPredicate);

        if (ignoreFilter)
            query = query.IgnoreQueryFilters();
        if (include is not null)
            query = include(query);
        if (orderBy is not null)
            query = orderBy(query);
        if (take is not null)
            query = query.Take(take.Value);
        if (asSplit)
            query = query.AsSplitQuery();

        if (select is not null)
            return query.Select(select).ToListAsync(cancellationToken);

        return query.AsNoTrackingWithIdentityResolution().Cast<TResult>().ToListAsync(cancellationToken);
    }

    public Task<List<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (orderBy is not null)
            query = orderBy(query);

        return query.Where(a => !a.IsDeleted).ToListAsync(cancellationToken);
    }

    public Task<IQueryable<TEntity>> GetAllQueryableAsync(
        Expression<Func<TEntity, bool>> filterPredicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool withDeleted = false,
        bool asSplit = false,
        bool asNoTracking = false)
    {
        var query = _dbSet.AsQueryable();

        if (!withDeleted)
            query = query.Where(a => !a.IsDeleted);
        if (include is not null)
            query = include(query);
        if (orderBy is not null)
            query = orderBy(query);
        if (asSplit)
            query = query.AsSplitQuery();
        if (asNoTracking)
            query = query.AsNoTracking();

        query = query.Where(filterPredicate);

        return Task.FromResult(query);
    }

    public async Task<bool> IsExistsAsync(
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(a => !a.IsDeleted);

        if (filterPredicate is not null)
            return await query.AsNoTracking().AnyAsync(filterPredicate, cancellationToken);

        return await query.AsNoTracking().AnyAsync(cancellationToken);
    }

    public async Task<bool> IsExistsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var idList = ids.ToList();
        if (idList.Count == 0)
            return true;

        var existingIds = await _dbSet
            .Where(a => !a.IsDeleted)
            .AsNoTracking()
            .Where(e => idList.Contains(e.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        return existingIds.Count == idList.Count;
    }

    public async Task<(List<TEntity> Items, int Count)> GetPaginatedAsync(
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Pagination? pagination = null,
        Expression<Func<TEntity, TEntity>>? select = null,
        CancellationToken cancellationToken = default)
    {
        var count = 0;
        var query = _dbSet.AsNoTracking().AsQueryable();
        query = query.Where(a => !a.IsDeleted);

        if (include is not null)
            query = include(query);
        if (orderBy is not null)
            query = orderBy(query);
        if (filterPredicate is not null)
            query = query.Where(filterPredicate);
        if (select is not null)
            query = query.Select(select);

        if (pagination is not null)
        {
            count = await query.CountAsync(cancellationToken);
            query = query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize);
        }

        return (await query.ToListAsync(cancellationToken), count);
    }

    public async Task<PaginationResponse<TResult>> GetPaginatedAsync<TResult>(
        Pagination pagination,
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Expression<Func<TEntity, TResult>>? select = null,
        bool withDeleted = false,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (!withDeleted)
            query = query.Where(e => !e.IsDeleted);
        if (filterPredicate is not null)
            query = query.Where(filterPredicate);
        if (include is not null)
            query = include(query);
        if (asNoTracking)
            query = query.AsNoTracking();
        if (orderBy is not null)
            query = orderBy(query);

        var totalCount = await query.CountAsync(cancellationToken);

        var pagedQuery = query.Paginate(pagination);

        List<TResult> items;
        if (select is not null)
            items = await pagedQuery.Select(select).ToListAsync(cancellationToken);
        else
            items = await pagedQuery.Cast<TResult>().ToListAsync(cancellationToken);

        return new PaginationResponse<TResult>
        {
            Count = totalCount,
            Data = items
        };
    }

    public IQueryable<TEntity> GetPaginatedQuerableAsync(
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Pagination? pagination = null,
        Expression<Func<TEntity, TEntity>>? select = null,
        bool withDeleted = false)
    {
        var query = _dbSet.AsQueryable();

        if (!withDeleted)
            query = query.Where(a => !a.IsDeleted);
        if (include is not null)
            query = include(query);
        if (orderBy is not null)
            query = orderBy(query);
        if (filterPredicate is not null)
            query = query.Where(filterPredicate);
        if (select is not null)
            query = query.Select(select);
        if (pagination is not null)
            query = query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize);

        return query;
    }

    public Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        bool withDeleted = false,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable().AsNoTracking();

        if (!withDeleted)
            query = query.Where(a => !a.IsDeleted);
        if (filterPredicate is not null)
            query = query.Where(filterPredicate);

        return query.CountAsync(cancellationToken);
    }

    public Task<decimal> SumAsync(
        Expression<Func<TEntity, decimal>> sum,
        Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        if (filter is not null)
            query = query.Where(filter);

        return query.SumAsync(sum, cancellationToken);
    }

    public Task<int> DeleteAsync(
        Expression<Func<TEntity, bool>> filterPredicate,
        CancellationToken cancellationToken = default) =>
        _dbSet
            .Where(a => !a.IsDeleted)
            .Where(filterPredicate)
            .ExecuteDeleteAsync(cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
