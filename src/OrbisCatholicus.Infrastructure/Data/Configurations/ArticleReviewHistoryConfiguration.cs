using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data.Configurations;

public class ArticleReviewHistoryConfiguration : IEntityTypeConfiguration<ArticleReviewHistory>
{
    public void Configure(EntityTypeBuilder<ArticleReviewHistory> builder)
    {
        builder.ToTable("article_review_history");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ArticleVersionId).HasColumnName("article_version_id");
        builder.Property(e => e.ReviewerId).HasColumnName("reviewer_id");
        builder.Property(e => e.PreviousStatusId).HasColumnName("previous_status_id");
        builder.Property(e => e.NewStatusId).HasColumnName("new_status_id");
        builder.Property(e => e.Notes).HasColumnName("notes");
        builder.Property(e => e.ReviewedAt).HasColumnName("reviewed_at");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne(e => e.ArticleVersion)
            .WithMany(v => v.ReviewHistories)
            .HasForeignKey(e => e.ArticleVersionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Reviewer)
            .WithMany()
            .HasForeignKey(e => e.ReviewerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.PreviousStatus)
            .WithMany()
            .HasForeignKey(e => e.PreviousStatusId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.NewStatus)
            .WithMany()
            .HasForeignKey(e => e.NewStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
