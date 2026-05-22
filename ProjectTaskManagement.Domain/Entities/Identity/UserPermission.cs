using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Domain.Entities.Identity;

public class UserPermission : BaseEntity
{
    public string User_Id { get; set; } = string.Empty;
    public string Permission { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
    public Permission PermissionDefinition { get; set; } = null!;
}
