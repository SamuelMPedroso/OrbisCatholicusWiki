namespace OrbisCatholicus.Domain.Entities;

public class ArticleTag
{
    public int Id { get; set; }
    public int ArticleId { get; set; }
    public string Tag { get; set; } = string.Empty;

    // Navigation
    public Article Article { get; set; } = null!;
}
