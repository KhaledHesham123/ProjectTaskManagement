using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskHandler(
    IGenericRepository<TaskItem> taskRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteTaskCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteTaskCommand request,
        CancellationToken cancellationToken)
    {
        var task = await taskRepository
            .GetByCriteriaQueryable(t => t.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (task is null)
            return Result<bool>.Fail("Task not found.");

        taskRepository.Delete(task);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
