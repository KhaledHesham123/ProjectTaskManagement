using MediatR;
using ProjectTaskManagement.Application.Features.Projects.Dtos;

namespace ProjectTaskManagement.Application.Features.Projects.Queries.GetProjectById;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDto>;
