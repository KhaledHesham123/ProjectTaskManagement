using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Tasks.Dtos;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Tasks.Queries.GetTasksByProject;

public class GetTasksByProjectHandler(IGenericRepository<TaskItem> taskRepository)
    : IRequestHandler<GetTasksByProjectQuery, Result<IReadOnlyList<TaskItemDto>>>
{
    public async Task<Result<IReadOnlyList<TaskItemDto>>> Handle(
        GetTasksByProjectQuery request,
        CancellationToken cancellationToken)
    {
        var tasks = await taskRepository
            .GetByCriteriaQueryable(t => t.ProjectId == request.ProjectId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);

        var data = tasks
            .Select(t => new TaskItemDto(
                t.Id,
                t.Title,
                t.Description,
                t.Status,
                t.Priority,
                t.DueDate,
                t.ProjectId))
            .ToList();

        return Result<IReadOnlyList<TaskItemDto>>.Success(data);
    }
}
