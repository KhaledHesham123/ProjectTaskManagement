using ProjectTaskManagement.Application.Features.Tasks.Dtos;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Enums;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    string Title,
    string? Description,
    PriorityLevel Priority,
    DateTime? DueDate,
    Guid ProjectId) : ICommand<TaskItemDto>;
