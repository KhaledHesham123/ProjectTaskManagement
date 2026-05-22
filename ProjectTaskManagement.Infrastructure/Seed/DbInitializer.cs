using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Entities.Identity;
using ProjectTaskManagement.Infrastructure.Persistence;

namespace ProjectTaskManagement.Infrastructure.Seed;

public static class DbInitializer
{
    public static async Task InitializeDatabase(this IApplicationBuilder app)
    {
        try
        {
            using var scope = app
                .ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await SeedDatabase.Seed(unitOfWork, context, userManager, roleManager);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while initializing the database.", ex);
        }
    }
}
