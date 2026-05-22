using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    bool HasActiveTransaction { get; }

    IGenericRepository<T> Repository<T>() where T : BaseEntity;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
