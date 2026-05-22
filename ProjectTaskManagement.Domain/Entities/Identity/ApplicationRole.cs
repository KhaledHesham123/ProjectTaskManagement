using Microsoft.AspNetCore.Identity;

namespace ProjectTaskManagement.Domain.Entities.Identity;

public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }
    public DateTime Created_At { get; set; }
    public bool Is_Deleted { get; set; }

    public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}
