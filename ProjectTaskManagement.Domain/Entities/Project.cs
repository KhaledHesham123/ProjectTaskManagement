using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = [];
}
