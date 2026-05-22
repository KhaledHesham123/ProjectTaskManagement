using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand(
    string Name,
    string? Description) : ICommand<bool>;
