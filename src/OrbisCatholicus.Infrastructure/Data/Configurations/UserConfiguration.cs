using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Username).HasColumnName("username").HasMaxLength(100).IsRequired();
        builder.Property(e => e.DisplayName).HasColumnName("display_name").HasMaxLength(150).IsRequired();
        builder.Property(e => e.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        builder.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
        builder.Property(e => e.AvatarUrl).HasColumnName("avatar_url").HasMaxLength(500);
        builder.Property(e => e.Bio).HasColumnName("bio");
        builder.Property(e => e.AccessLevelId).HasColumnName("access_level_id");
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.EmailConfirmed).HasColumnName("email_confirmed").HasDefaultValue(false);
        builder.Property(e => e.LastLoginAt).HasColumnName("last_login_at");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.DeactivatedAt).HasColumnName("deactivated_at");
        builder.Property(e => e.RefreshToken).HasColumnName("refresh_token").HasMaxLength(500);
        builder.Property(e => e.RefreshTokenExpiryTime).HasColumnName("refresh_token_expiry_time");

        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.Username).IsUnique();

        builder.HasOne(e => e.AccessLevel)
            .WithMany(a => a.Users)
            .HasForeignKey(e => e.AccessLevelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
