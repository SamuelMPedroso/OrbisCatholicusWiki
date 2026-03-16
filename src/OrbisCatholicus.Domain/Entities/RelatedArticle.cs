namespace OrbisCatholicus.Domain.Entities;

public class RelatedArticle
{
    public int Id { get; set; }
    public int ArticleId { get; set; }
    public int RelatedArticleId { get; set; }
    public string RelationType { get; set; } = "ver_tambem";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Article Article { get; set; } = null!;
    public Article Related { get; set; } = null!;
}
