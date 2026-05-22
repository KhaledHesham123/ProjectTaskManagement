using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities;
using ProjectTaskManagement.Domain.Entities.Auth;
using ProjectTaskManagement.Domain.Entities.Identity;

namespace ProjectTaskManagement.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUser)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
        ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInformation()
    {
        var userId = currentUser.UserId ?? "system";
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        baseEntity.CreatedAt = now;
                        baseEntity.CreatedBy = userId;
                        baseEntity.IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        baseEntity.ModifiedAt = now;
                        baseEntity.ModifiedBy = userId;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        baseEntity.IsDeleted = true;
                        baseEntity.ModifiedAt = now;
                        baseEntity.ModifiedBy = userId;
                        break;
                }
            }
            else if (entry.Entity is ApplicationUser user)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        user.Created_At = now;
                        user.Created_By = userId;
                        user.Is_Deleted = false;
                        break;
                    case EntityState.Modified:
                        user.Modified_At = now;
                        user.Modified_By = userId;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        user.Is_Deleted = true;
                        user.Modified_At = now;
                        user.Modified_By = userId;
                        break;
                }
            }
        }
    }
}
