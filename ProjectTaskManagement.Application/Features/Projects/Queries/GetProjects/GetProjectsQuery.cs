using MediatR;
using ProjectTaskManagement.Application.Features.Projects.Dtos;
using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Features.Projects.Queries.GetProjects;

public record GetProjectsQuery : IRequest<Result<IReadOnlyList<ProjectDto>>>;
