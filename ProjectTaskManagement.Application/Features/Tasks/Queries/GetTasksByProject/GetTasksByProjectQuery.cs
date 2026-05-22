using MediatR;
using ProjectTaskManagement.Application.Features.Tasks.Dtos;

namespace ProjectTaskManagement.Application.Features.Tasks.Queries.GetTasksByProject;

public record GetTasksByProjectQuery(Guid ProjectId) : IRequest<IReadOnlyList<TaskItemDto>>;
