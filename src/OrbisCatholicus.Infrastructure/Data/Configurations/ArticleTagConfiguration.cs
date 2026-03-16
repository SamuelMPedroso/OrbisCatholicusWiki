using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data.Configurations;

public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTag>
{
    public void Configure(EntityTypeBuilder<ArticleTag> builder)
    {
        builder.ToTable("article_tags");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.Tag).HasColumnName("tag").HasMaxLength(50).IsRequired();

        builder.HasIndex(e => new { e.ArticleId, e.Tag }).IsUnique();

        builder.HasOne(e => e.Article)
            .WithMany(a => a.Tags)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
