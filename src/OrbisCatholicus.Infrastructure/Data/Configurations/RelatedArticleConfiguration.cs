using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data.Configurations;

public class RelatedArticleConfiguration : IEntityTypeConfiguration<RelatedArticle>
{
    public void Configure(EntityTypeBuilder<RelatedArticle> builder)
    {
        builder.ToTable("related_articles");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.RelatedArticleId).HasColumnName("related_article_id");
        builder.Property(e => e.RelationType).HasColumnName("relation_type").HasMaxLength(50).HasDefaultValue("ver_tambem");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(e => new { e.ArticleId, e.RelatedArticleId }).IsUnique();

        builder.HasOne(e => e.Article)
            .WithMany(a => a.RelatedArticles)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Related)
            .WithMany()
            .HasForeignKey(e => e.RelatedArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(t => t.HasCheckConstraint("CK_related_articles_no_self", "\"article_id\" != \"related_article_id\""));
    }
}
