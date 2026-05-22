using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTaskManagement.Domain.Entities.Identity;

namespace ProjectTaskManagement.Infrastructure.Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Permission).IsRequired().HasMaxLength(200);
        builder.HasIndex(p => new { p.Role_Id, p.Permission }).IsUnique();
    }
}
