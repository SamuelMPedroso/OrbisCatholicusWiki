namespace OrbisCatholicus.Domain.Entities;

public class ArticleReference : BaseEntity
{
    public int ArticleId { get; set; }
    public string ReferenceType { get; set; } = "bibliografica";
    public string ReferenceText { get; set; } = string.Empty;
    public string? ReferenceUrl { get; set; }
    public int DisplayOrder { get; set; }

    // Navigation
    public Article Article { get; set; } = null!;
}
