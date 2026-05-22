using System.Linq.Expressions;
using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Common.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void SaveInclude(TEntity entity, params string[] includedProperties);
    void Delete(TEntity entity);

    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetByCriteriaQueryable(Expression<Func<TEntity, bool>> predicate);

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
}
