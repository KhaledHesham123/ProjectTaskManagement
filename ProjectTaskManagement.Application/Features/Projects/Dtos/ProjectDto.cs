namespace ProjectTaskManagement.Application.Features.Projects.Dtos;

public record ProjectDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt);
