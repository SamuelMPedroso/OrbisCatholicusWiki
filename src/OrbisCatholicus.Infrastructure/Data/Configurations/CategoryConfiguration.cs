using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(e => e.Slug).HasColumnName("slug").HasMaxLength(120).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.IconClass).HasColumnName("icon_class").HasMaxLength(100);
        builder.Property(e => e.ParentCategoryId).HasColumnName("parent_category_id");
        builder.Property(e => e.DisplayOrder).HasColumnName("display_order").HasDefaultValue(0);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.DeactivatedAt).HasColumnName("deactivated_at");

        builder.HasIndex(e => e.Name).IsUnique();
        builder.HasIndex(e => e.Slug).IsUnique();

        builder.HasOne(e => e.ParentCategory)
            .WithMany(e => e.SubCategories)
            .HasForeignKey(e => e.ParentCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(
            new Category { Id = 1, Name = "Doutrina", Slug = "doutrina", Description = "Ensinamentos da fé católica, dogmas e teologia", IconClass = "fas fa-book-bible", DisplayOrder = 1 },
            new Category { Id = 2, Name = "Santos e Beatos", Slug = "santos-e-beatos", Description = "Biografias e hagiografias dos santos da Igreja", IconClass = "fas fa-cross", DisplayOrder = 2 },
            new Category { Id = 3, Name = "Liturgia", Slug = "liturgia", Description = "Ritos, sacramentos e celebrações litúrgicas", IconClass = "fas fa-church", DisplayOrder = 3 },
            new Category { Id = 4, Name = "História da Igreja", Slug = "historia-da-igreja", Description = "Eventos históricos e períodos da história eclesiástica", IconClass = "fas fa-landmark", DisplayOrder = 4 },
            new Category { Id = 5, Name = "Papado", Slug = "papado", Description = "Papas, conclaves e a Sé Apostólica", IconClass = "fas fa-crown", DisplayOrder = 5 },
            new Category { Id = 6, Name = "Ordens Religiosas", Slug = "ordens-religiosas", Description = "Congregações, ordens monásticas e institutos religiosos", IconClass = "fas fa-hands-praying", DisplayOrder = 6 },
            new Category { Id = 7, Name = "Sacramentos", Slug = "sacramentos", Description = "Os sete sacramentos e sua teologia", IconClass = "fas fa-dove", DisplayOrder = 7 },
            new Category { Id = 8, Name = "Direito Canônico", Slug = "direito-canonico", Description = "Legislação eclesiástica e normas canônicas", IconClass = "fas fa-scale-balanced", DisplayOrder = 8 },
            new Category { Id = 9, Name = "Filosofia Cristã", Slug = "filosofia-crista", Description = "Pensamento filosófico e teológico cristão", IconClass = "fas fa-lightbulb", DisplayOrder = 9 },
            new Category { Id = 10, Name = "Arte Sacra", Slug = "arte-sacra", Description = "Iconografia, arquitetura e arte religiosa", IconClass = "fas fa-palette", DisplayOrder = 10 },
            new Category { Id = 11, Name = "Mariologia", Slug = "mariologia", Description = "Estudo e devoção à Virgem Maria", IconClass = "fas fa-star", DisplayOrder = 11 },
            new Category { Id = 12, Name = "Concílios", Slug = "concilios", Description = "Concílios ecumênicos e regionais da Igreja", IconClass = "fas fa-users", DisplayOrder = 12 }
        );
    }
}
