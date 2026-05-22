using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(Guid Id) : ICommand<Result<bool>>;
