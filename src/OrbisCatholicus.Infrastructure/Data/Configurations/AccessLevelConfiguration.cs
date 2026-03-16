using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data.Configurations;

public class AccessLevelConfiguration : IEntityTypeConfiguration<AccessLevel>
{
    public void Configure(EntityTypeBuilder<AccessLevel> builder)
    {
        builder.ToTable("access_levels");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(e => e.Name).IsUnique();

        builder.HasData(
            new AccessLevel { Id = 1, Name = "Leitor", Description = "Pode visualizar artigos e comentar" },
            new AccessLevel { Id = 2, Name = "Autor", Description = "Pode criar e editar artigos" },
            new AccessLevel { Id = 3, Name = "Revisor", Description = "Pode aprovar ou rejeitar artigos pendentes" },
            new AccessLevel { Id = 4, Name = "Administrador", Description = "Acesso total ao sistema" }
        );
    }
}
