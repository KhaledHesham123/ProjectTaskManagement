using MediatR;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Tasks.Dtos;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskHandler(
    IGenericRepository<Project> projectRepository,
    IGenericRepository<TaskItem> taskRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTaskCommand, Result<TaskItemDto>>
{
    public async Task<Result<TaskItemDto>> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken)
    {
        var projectExists = await projectRepository.AnyAsync(
            p => p.Id == request.ProjectId,
            cancellationToken);

        if (!projectExists)
            return Result<TaskItemDto>.Fail("Project not found.");

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId
        };

        await taskRepository.AddAsync(task, cancellationToken);
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
