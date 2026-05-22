using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTaskManagement.Domain.Entities.Identity;

namespace ProjectTaskManagement.Infrastructure.Persistence.Configurations;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.Property(r => r.Description).HasMaxLength(500);
        builder.HasQueryFilter(r => !r.Is_Deleted);

        builder.HasMany(r => r.RolePermissions)
            .WithOne(p => p.Role)
            .HasForeignKey(p => p.Role_Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
