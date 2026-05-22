using MediatR;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Tasks.Dtos;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Tasks.Queries.GetTasksByProject;

public class GetTasksByProjectHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetTasksByProjectQuery, IReadOnlyList<TaskItemDto>>
{
    public async Task<IReadOnlyList<TaskItemDto>> Handle(
        GetTasksByProjectQuery request,
        CancellationToken cancellationToken)
    {
        var tasks = await unitOfWork.Repository<TaskItem>().GetAllAsync(
            t => t.ProjectId == request.ProjectId,
            orderBy: q => q.OrderByDescending(t => t.DueDate),
            cancellationToken: cancellationToken);

        return tasks
            .Select(t => new TaskItemDto(
                t.Id,
                t.Title,
                t.Description,
                t.Status,
                t.Priority,
                t.DueDate,
                t.ProjectId))
            .ToList();
    }
}
