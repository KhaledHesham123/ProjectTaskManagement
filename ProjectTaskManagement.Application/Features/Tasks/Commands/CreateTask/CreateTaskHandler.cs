using MediatR;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskHandler(
    IGenericRepository<Project> projectRepository,
    IGenericRepository<TaskItem> taskRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTaskCommand, bool>
{
    public async Task<bool> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var projectExists = await projectRepository.AnyAsync(
            p => p.Id == request.ProjectId,
            cancellationToken);

        if (!projectExists)
            return false;

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

        return true;
    }
}
