using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data.Configurations;

public class ReviewStatusConfiguration : IEntityTypeConfiguration<ReviewStatus>
{
    public void Configure(EntityTypeBuilder<ReviewStatus> builder)
    {
        builder.ToTable("review_status");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.ColorHex).HasColumnName("color_hex").HasMaxLength(7).HasDefaultValue("#6c757d");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(e => e.Name).IsUnique();

        builder.HasData(
            new ReviewStatus { Id = 1, Name = "Rascunho", Description = "Artigo em elaboração pelo autor", ColorHex = "#6c757d" },
            new ReviewStatus { Id = 2, Name = "Pendente", Description = "Aguardando revisão de um moderador", ColorHex = "#ffc107" },
            new ReviewStatus { Id = 3, Name = "Em Revisão", Description = "Sendo revisado por um moderador", ColorHex = "#17a2b8" },
            new ReviewStatus { Id = 4, Name = "Aprovado", Description = "Aprovado e publicado", ColorHex = "#28a745" },
            new ReviewStatus { Id = 5, Name = "Rejeitado", Description = "Rejeitado pelo revisor, necessita correções", ColorHex = "#dc3545" },
            new ReviewStatus { Id = 6, Name = "Revisão Solicitada", Description = "Autor solicitou nova revisão após correções", ColorHex = "#fd7e14" }
        );
    }
}
