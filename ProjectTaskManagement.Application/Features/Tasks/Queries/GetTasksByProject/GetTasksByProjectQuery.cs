using MediatR;
using ProjectTaskManagement.Application.Features.Tasks.Dtos;
using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Features.Tasks.Queries.GetTasksByProject;

public record GetTasksByProjectQuery(Guid ProjectId) : IRequest<Result<IReadOnlyList<TaskItemDto>>>;
