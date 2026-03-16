using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data.Configurations;

public class ArticleReferenceConfiguration : IEntityTypeConfiguration<ArticleReference>
{
    public void Configure(EntityTypeBuilder<ArticleReference> builder)
    {
        builder.ToTable("article_references");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.ReferenceType).HasColumnName("reference_type").HasMaxLength(50).HasDefaultValue("bibliografica");
        builder.Property(e => e.ReferenceText).HasColumnName("reference_text").IsRequired();
        builder.Property(e => e.ReferenceUrl).HasColumnName("reference_url").HasMaxLength(500);
        builder.Property(e => e.DisplayOrder).HasColumnName("display_order").HasDefaultValue(0);
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne(e => e.Article)
            .WithMany(a => a.References)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
