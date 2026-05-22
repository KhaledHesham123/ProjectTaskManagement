using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Common.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    void SaveInclude(TEntity entity, params string[] includedProperties);
    void Delete(TEntity entity);

    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetByCriteriaQueryable(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> filterPredicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Expression<Func<TEntity, TEntity>>? select = null,
        bool asNoTracking = false,
        bool asSplit = false,
        bool ignoreFilter = false,
        bool withDeleted = false,
        CancellationToken cancellationToken = default);

    Task<TResult?> FindAsync<TResult>(
        Expression<Func<TEntity, bool>> filterPredicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool asNoTracking = false,
        Expression<Func<TEntity, TResult>>? select = null,
        bool asSplit = false,
        bool ignoreFilter = false,
        bool withDeleted = false,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> filterPredicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Expression<Func<TEntity, TEntity>>? select = null,
        int? take = null,
        bool ignoreFilter = false,
        bool asSplit = false,
        CancellationToken cancellationToken = default);

    Task<List<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, bool>> filterPredicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Expression<Func<TEntity, TResult>>? select = null,
        int? take = null,
        bool ignoreFilter = false,
        bool asSplit = false,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default);

    Task<IQueryable<TEntity>> GetAllQueryableAsync(
        Expression<Func<TEntity, bool>> filterPredicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool withDeleted = false,
        bool asSplit = false,
        bool asNoTracking = false);

    Task<bool> IsExistsAsync(
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        CancellationToken cancellationToken = default);

    Task<bool> IsExistsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);

    Task<(List<TEntity> Items, int Count)> GetPaginatedAsync(
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Pagination? pagination = null,
        Expression<Func<TEntity, TEntity>>? select = null,
        CancellationToken cancellationToken = default);

    Task<PaginationResponse<TResult>> GetPaginatedAsync<TResult>(
        Pagination pagination,
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Expression<Func<TEntity, TResult>>? select = null,
        bool withDeleted = false,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    IQueryable<TEntity> GetPaginatedQuerableAsync(
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Pagination? pagination = null,
        Expression<Func<TEntity, TEntity>>? select = null,
        bool withDeleted = false);

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? filterPredicate = null,
        bool withDeleted = false,
        CancellationToken cancellationToken = default);

    Task<decimal> SumAsync(
        Expression<Func<TEntity, decimal>> sum,
        Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default);

    Task<int> DeleteAsync(
        Expression<Func<TEntity, bool>> filterPredicate,
        CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
