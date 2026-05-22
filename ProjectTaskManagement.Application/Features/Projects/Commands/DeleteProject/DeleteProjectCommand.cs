using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Features.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(Guid Id) : ICommand<Result<bool>>;
