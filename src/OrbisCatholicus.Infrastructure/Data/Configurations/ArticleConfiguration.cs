using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
        builder.Property(e => e.Slug).HasColumnName("slug").HasMaxLength(300).IsRequired();
        builder.Property(e => e.Summary).HasColumnName("summary");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");
        builder.Property(e => e.CurrentVersionId).HasColumnName("current_version_id");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.FeaturedImageUrl).HasColumnName("featured_image_url").HasMaxLength(500);
        builder.Property(e => e.IsFeatured).HasColumnName("is_featured").HasDefaultValue(false);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.Views).HasColumnName("views").HasDefaultValue(0);
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.DeactivatedAt).HasColumnName("deactivated_at");

        builder.HasIndex(e => e.Slug).IsUnique();

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Articles)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Author)
            .WithMany(u => u.Articles)
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.CurrentVersion)
            .WithOne()
            .HasForeignKey<Article>(e => e.CurrentVersionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
