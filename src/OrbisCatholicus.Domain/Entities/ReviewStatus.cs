namespace OrbisCatholicus.Domain.Entities;

public class ReviewStatus : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ColorHex { get; set; } = "#6c757d";

    // Navigation
    public ICollection<ArticleVersion> ArticleVersions { get; set; } = [];
}
