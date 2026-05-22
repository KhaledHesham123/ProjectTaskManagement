using ProjectTaskManagement.Domain.Enums;

namespace ProjectTaskManagement.Application.Features.Tasks.Dtos;

public record TaskItemDto(
    Guid Id,
    string Title,
    string? Description,
    TaskItemStatus Status,
    PriorityLevel Priority,
    DateTime? DueDate,
    Guid ProjectId);
