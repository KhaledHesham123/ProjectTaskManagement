using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities.Identity;
using ProjectTaskManagement.Infrastructure.Persistence;
using static ProjectTaskManagement.Domain.Common.ApplicationConstants;

namespace ProjectTaskManagement.Infrastructure.Seed;

public static class SeedDatabase
{
    public static async Task Seed(
        IUnitOfWork unitOfWork,
        AppDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        await SeedRoleData(roleManager);
        await SeedingPermissionsAsync(context);
        await SeedAdminUserAsync(context, userManager);
        await SeedAdminPermissionsAsync(context);

        await unitOfWork.SaveChangesAsync();
    }

    private static async Task SeedAdminUserAsync(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        var admin = new ApplicationUser
        {
            Id = AdminId,
            Email = "Admin@Admin.com",
            NormalizedEmail = "ADMIN@ADMIN.COM",
            UserName = "Admin",
            NormalizedUserName = "ADMIN",
            Is_Active = true,
            PhoneNumber = "01000000000",
            JobTitle = "Administrator",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        var set = context.Set<ApplicationUser>();

        if (await set.AnyAsync(a => a.Id == AdminId || a.NormalizedUserName == admin.NormalizedUserName))
            return;

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        admin.PasswordHash = passwordHasher.HashPassword(admin, "P00000");
        await set.AddAsync(admin);
        await context.SaveChangesAsync();
        await userManager.AddToRoleAsync(admin, nameof(UserRole.SuperAdmin));
    }

    private static async Task SeedAdminPermissionsAsync(AppDbContext context)
    {
        var admin = await context.Users
            .FirstOrDefaultAsync(u => u.NormalizedEmail == "ADMIN@ADMIN.COM");

        if (admin is null)
            return;

        var permissions = new[]
        {
            AppPermissions.View,
            AppPermissions.Create,
            AppPermissions.Edit,
            AppPermissions.Delete
        };

        foreach (var permission in permissions)
        {
            var exists = await context.UserPermissions.AnyAsync(
                up => up.User_Id == admin.Id && up.Permission == permission);

            if (exists)
                continue;

            await context.UserPermissions.AddAsync(new UserPermission
            {
                Id = Guid.NewGuid(),
                User_Id = admin.Id,
                Permission = permission
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedRoleData(RoleManager<ApplicationRole> roleManager)
    {
        foreach (UserRole role in Enum.GetValues<UserRole>())
        {
            if (!await roleManager.RoleExistsAsync(role.ToString()))
                await roleManager.CreateAsync(new ApplicationRole { Name = role.ToString() });
        }
    }

    private static async Task SeedingPermissionsAsync(AppDbContext context)
    {
        if (await context.Permissions.AnyAsync())
            return;

        await context.Permissions.AddRangeAsync(
        [
            new Permission
            {
                Id = Guid.Parse(AppPermissions.ViewId),
                Name = AppPermissions.View
            },
            new Permission
            {
                Id = Guid.Parse(AppPermissions.CreateId),
                Name = AppPermissions.Create
            },
            new Permission
            {
                Id = Guid.Parse(AppPermissions.EditId),
                Name = AppPermissions.Edit
            },
            new Permission
            {
                Id = Guid.Parse(AppPermissions.DeleteId),
                Name = AppPermissions.Delete
            }
        ]);

        await context.SaveChangesAsync();
    }
}
