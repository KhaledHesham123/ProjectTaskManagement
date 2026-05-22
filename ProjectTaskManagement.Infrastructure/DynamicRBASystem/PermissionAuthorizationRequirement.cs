using Microsoft.AspNetCore.Authorization;

namespace ProjectTaskManagement.Infrastructure.DynamicRBASystem;

public class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public string[] AllowedPermissions { get; }
    public bool RequireAll { get; }

    public PermissionAuthorizationRequirement(string[] allowedPermissions, bool requireAll = false)
    {
        AllowedPermissions = allowedPermissions;
        RequireAll = requireAll;
    }
}
