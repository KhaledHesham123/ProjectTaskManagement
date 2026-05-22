using MediatR;
using ProjectTaskManagement.Application.Common.Exceptions;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Tasks.Dtos;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateTaskCommand, TaskItemDto>
{
    public async Task<TaskItemDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var projectExists = await unitOfWork.Repository<Project>().IsExistsAsync(
            p => p.Id == request.ProjectId,
            cancellationToken);

        if (!projectExists)
            throw new NotFoundException(nameof(Project), request.ProjectId);

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId
        };

        await unitOfWork.Repository<TaskItem>().AddAsync(task, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new TaskItemDto(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.Priority,
            task.DueDate,
            task.ProjectId);
    }
}
