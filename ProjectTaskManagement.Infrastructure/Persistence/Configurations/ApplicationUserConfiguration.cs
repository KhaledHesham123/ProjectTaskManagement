using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTaskManagement.Domain.Entities.Identity;

namespace ProjectTaskManagement.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Full_Name).HasMaxLength(200);
        builder.Property(u => u.First_Name).HasMaxLength(100);
        builder.Property(u => u.Last_Name).HasMaxLength(100);
        builder.Property(u => u.JobTitle).HasMaxLength(100);
        builder.Property(u => u.Created_By).HasMaxLength(450);
        builder.Property(u => u.Modified_By).HasMaxLength(450);
        builder.Property(u => u.Refresh_Token).HasMaxLength(500);

        builder.HasQueryFilter(u => !u.Is_Deleted);

        builder.HasMany(u => u.UserPermissions)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.User_Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
