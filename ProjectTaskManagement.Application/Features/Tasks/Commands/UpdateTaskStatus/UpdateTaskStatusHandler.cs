using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusHandler(
    IGenericRepository<TaskItem> taskRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateTaskStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository
            .GetByCriteriaQueryable(t => t.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (task is null)
            return false;

        task.Status = request.Status;

        taskRepository.SaveInclude(task, nameof(TaskItem.Status));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
