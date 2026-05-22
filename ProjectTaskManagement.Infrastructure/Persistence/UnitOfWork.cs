using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Infrastructure.Persistence.Repositories;

namespace ProjectTaskManagement.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly ConcurrentDictionary<Type, object> _repositories = new();
    private IDbContextTransaction? _transaction;

    public bool HasActiveTransaction => _transaction is not null;

    public IGenericRepository<T> Repository<T>() where T : BaseEntity =>
        (IGenericRepository<T>)_repositories.GetOrAdd(typeof(T), _ => new GenericRepository<T>(context));

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            return;

        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose() => _transaction?.Dispose();
}
