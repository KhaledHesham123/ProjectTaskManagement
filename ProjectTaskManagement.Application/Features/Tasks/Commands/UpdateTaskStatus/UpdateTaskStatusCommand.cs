using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Enums;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;

public record UpdateTaskStatusCommand(Guid Id, TaskItemStatus Status) : ICommand<bool>;
