using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data.Configurations;

public class ArticleVersionConfiguration : IEntityTypeConfiguration<ArticleVersion>
{
    public void Configure(EntityTypeBuilder<ArticleVersion> builder)
    {
        builder.ToTable("article_versions");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.VersionNumber).HasColumnName("version_number").HasDefaultValue(1);
        builder.Property(e => e.Content).HasColumnName("content").IsRequired();
        builder.Property(e => e.ContentHtml).HasColumnName("content_html").IsRequired();
        builder.Property(e => e.EditSummary).HasColumnName("edit_summary").HasMaxLength(500);
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.ReviewStatusId).HasColumnName("review_status_id");
        builder.Property(e => e.ReviewedBy).HasColumnName("reviewed_by");
        builder.Property(e => e.ReviewedAt).HasColumnName("reviewed_at");
        builder.Property(e => e.Notes).HasColumnName("notes");
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.DeactivatedAt).HasColumnName("deactivated_at");

        builder.HasIndex(e => new { e.ArticleId, e.VersionNumber }).IsUnique();

        builder.HasOne(e => e.Article)
            .WithMany(a => a.Versions)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Author)
            .WithMany()
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Reviewer)
            .WithMany()
            .HasForeignKey(e => e.ReviewedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.ReviewStatus)
            .WithMany(r => r.ArticleVersions)
            .HasForeignKey(e => e.ReviewStatusId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
