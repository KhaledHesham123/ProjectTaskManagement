using Microsoft.AspNetCore.Identity;
using ProjectTaskManagement.Domain.Entities.Auth;

namespace ProjectTaskManagement.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public string? JobTitle { get; set; }
    public bool Is_Active { get; set; } = true;
    public DateTime Created_At { get; set; }
    public string? Created_By { get; set; }
    public DateTime? Modified_At { get; set; }
    public string? Modified_By { get; set; }
    public bool Is_Deleted { get; set; }

    public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
    public ICollection<UserPermission> UserPermissions { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
