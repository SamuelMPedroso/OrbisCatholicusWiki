namespace OrbisCatholicus.Domain.Entities;

public class Category : SoftDeletableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconClass { get; set; }
    public int? ParentCategoryId { get; set; }
    public int DisplayOrder { get; set; }

    // Navigation
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = [];
    public ICollection<Article> Articles { get; set; } = [];
}
