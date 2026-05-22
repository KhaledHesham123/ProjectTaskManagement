using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTaskManagement.Domain.Entities.Identity;

namespace ProjectTaskManagement.Infrastructure.Persistence.Configurations;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.ToTable("UserPermissions");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Permission).IsRequired().HasMaxLength(200);
        builder.HasIndex(p => new { p.User_Id, p.Permission }).IsUnique();
    }
}
