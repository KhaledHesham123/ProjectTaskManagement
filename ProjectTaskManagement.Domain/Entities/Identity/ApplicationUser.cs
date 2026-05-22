using Microsoft.AspNetCore.Identity;

namespace ProjectTaskManagement.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public string? Full_Name { get; set; }
    public string? First_Name { get; set; }
    public string? Last_Name { get; set; }
    public string? JobTitle { get; set; }
    public bool Is_Active { get; set; } = true;
    public DateTime Created_At { get; set; }
    public string? Created_By { get; set; }
    public DateTime? Modified_At { get; set; }
    public string? Modified_By { get; set; }
    public bool Is_Deleted { get; set; }
    public string? Refresh_Token { get; set; }
    public DateTime? Refresh_Token_Expires_At { get; set; }

    public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
    public ICollection<UserPermission> UserPermissions { get; set; } = [];
}
