using MediatR;
using Microsoft.Extensions.Logging;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Common.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var isFirstTransaction = !unitOfWork.HasActiveTransaction;

        try
        {
            if (isFirstTransaction)
                await unitOfWork.BeginTransactionAsync(cancellationToken);

            var response = await next();

            if (isFirstTransaction)
            {
                if (response is IResult result && !result.Succeeded)
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                else
                    await unitOfWork.CommitTransactionAsync(cancellationToken);
            }

            return response;
        }
        catch (Exception ex)
        {
            if (isFirstTransaction)
            {
                logger.LogError(
                    ex,
                    "A critical error occurred. Rolling back transaction for {RequestName}.",
                    typeof(TRequest).Name);

                await unitOfWork.RollbackTransactionAsync(cancellationToken);
            }

            throw;
        }
    }
}
