using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Enums;

namespace ProjectTaskManagement.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;
    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    public DateTime? DueDate { get; set; }
    public Guid ProjectId { get; set; }

    public Project Project { get; set; } = null!;
}
