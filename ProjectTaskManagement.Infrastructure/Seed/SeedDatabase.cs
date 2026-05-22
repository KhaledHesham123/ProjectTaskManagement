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
        await SeedUserData(context, userManager);
        await SeedingPermissionsAsync(context);

        await unitOfWork.SaveChangesAsync();
    }

    private static async Task SeedUserData(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        var programmer = new ApplicationUser
        {
            Id = Programmer,
            Full_Name = "Super Admin",
            Email = "Programmer@programmer.com",
            NormalizedEmail = "PROGRAMMER@PROGRAMMER.COM",
            UserName = "Programmer",
            NormalizedUserName = "PROGRAMMER",
            PhoneNumber = "01000000000",
            Is_Active = true,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            JobTitle = "System Programmer",
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        var admin = new ApplicationUser
        {
            Id = AdminId,
            Full_Name = "Admin",
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

        if (!await set.AnyAsync(a => a.Id == Programmer || a.NormalizedUserName == programmer.NormalizedUserName))
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            programmer.PasswordHash = passwordHasher.HashPassword(programmer, "P00000");
            await set.AddAsync(programmer);
            await context.SaveChangesAsync();
            await userManager.AddToRoleAsync(programmer, nameof(UserRole.SuperAdmin));
        }

        if (!await set.AnyAsync(a => a.Id == AdminId || a.NormalizedUserName == admin.NormalizedUserName))
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            admin.PasswordHash = passwordHasher.HashPassword(admin, "P00000");
            await set.AddAsync(admin);
            await context.SaveChangesAsync();
            await userManager.AddToRoleAsync(admin, nameof(UserRole.SuperAdmin));
        }
    }

    private static async Task SeedRoleData(RoleManager<ApplicationRole> roleManager)
    {
        var roles = new List<ApplicationRole>();

        foreach (UserRole role in Enum.GetValues<UserRole>())
            roles.Add(new ApplicationRole { Name = role.ToString() });

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
                await roleManager.CreateAsync(role);
        }
    }

    private static async Task SeedingPermissionsAsync(AppDbContext context)
    {
        var permissionsToSeed = new List<Permission>
        {
            new()
            {
                Id = Guid.Parse(AppPermissions.ViewId),
                Name = AppPermissions.View
            },
            new()
            {
                Id = Guid.Parse(AppPermissions.CreateId),
                Name = AppPermissions.Create
            },
            new()
            {
                Id = Guid.Parse(AppPermissions.EditId),
                Name = AppPermissions.Edit
            },
            new()
            {
                Id = Guid.Parse(AppPermissions.DeleteId),
                Name = AppPermissions.Delete
            }
        };

        if (!await context.Permissions.AnyAsync())
        {
            await context.Permissions.AddRangeAsync(permissionsToSeed);
            await context.SaveChangesAsync();
        }
    }
}
