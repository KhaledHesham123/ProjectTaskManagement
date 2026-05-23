using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Tasks.Dtos;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusHandler(
    IGenericRepository<TaskItem> taskRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateTaskStatusCommand, Result<TaskItemDto>>
{
    public async Task<Result<TaskItemDto>> Handle(
        UpdateTaskStatusCommand request,
        CancellationToken cancellationToken)
    {
        var task = await taskRepository
            .GetByCriteriaQueryable(t => t.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (task is null)
            return Result<TaskItemDto>.Fail("Task not found.");

        task.Status = request.Status;

        taskRepository.SaveInclude(task, nameof(TaskItem.Status));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<TaskItemDto>.Success(
            new TaskItemDto(
                task.Id,
                task.Title,
                task.Description,
                task.Status,
                task.Priority,
                task.DueDate,
                task.ProjectId));
    }
}
