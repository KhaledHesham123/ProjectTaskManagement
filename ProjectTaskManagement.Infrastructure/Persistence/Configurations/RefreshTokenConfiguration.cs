using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTaskManagement.Domain.Entities.Auth;

namespace ProjectTaskManagement.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.User_Id)
            .IsRequired()
            .HasMaxLength(450);

        builder.Ignore(t => t.Is_Expired);

        builder.HasIndex(t => t.Token).IsUnique();

        builder.HasQueryFilter(t => !t.IsDeleted);

        builder.HasOne(t => t.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(t => t.User_Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
