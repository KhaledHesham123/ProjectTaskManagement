using MediatR;
using ProjectTaskManagement.Application.Features.Projects.Dtos;

namespace ProjectTaskManagement.Application.Features.Projects.Queries.GetProjects;

public record GetProjectsQuery : IRequest<IReadOnlyList<ProjectDto>>;
