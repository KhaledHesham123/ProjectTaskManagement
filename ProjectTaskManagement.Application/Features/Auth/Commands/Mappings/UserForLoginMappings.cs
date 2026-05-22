using ProjectTaskManagement.Application.Features.Auth.Dtos;
using ProjectTaskManagement.Domain.Entities.Identity;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.Mappings;

public static class UserForLoginMappings
{
    public static UserTokenProjection ToUserTokenProjection(ApplicationUser user)
    {
        return new UserTokenProjection(
            user.Id,
            user.UserName ?? string.Empty,
            user.Email ?? string.Empty,
            user.Is_Active,
            user.UserRoles
                .Where(ur => ur.Role != null && ur.Role.Name != null)
                .Select(ur => ur.Role!.Name!)
                .ToList(),
            user.UserPermissions
                .Where(up => !up.IsDeleted)
                .Select(up => up.Permission)
                .Distinct()
                .ToList());
    }
}
