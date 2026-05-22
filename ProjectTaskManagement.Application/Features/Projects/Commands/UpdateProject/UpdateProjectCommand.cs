using ProjectTaskManagement.Application.Features.Projects.Dtos;
using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(
    Guid Id,
    string Name,
    string? Description) : ICommand<Result<ProjectDto>>;
