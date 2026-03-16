using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data.Configurations;

public class CommentReactionConfiguration : IEntityTypeConfiguration<CommentReaction>
{
    public void Configure(EntityTypeBuilder<CommentReaction> builder)
    {
        builder.ToTable("comment_reactions");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.CommentId).HasColumnName("comment_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.ReactionType).HasColumnName("reaction_type").HasMaxLength(10).IsRequired();
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(e => new { e.CommentId, e.UserId }).IsUnique();

        builder.ToTable(t => t.HasCheckConstraint("CK_comment_reactions_type", "\"reaction_type\" IN ('like', 'dislike')"));

        builder.HasOne(e => e.Comment)
            .WithMany(c => c.Reactions)
            .HasForeignKey(e => e.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
