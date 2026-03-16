namespace OrbisCatholicus.Domain.Entities;

public class Article : SoftDeletableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public int? CategoryId { get; set; }
    public int? CurrentVersionId { get; set; }
    public int CreatedBy { get; set; }
    public string? FeaturedImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int Views { get; set; }

    // Navigation
    public Category? Category { get; set; }
    public ArticleVersion? CurrentVersion { get; set; }
    public User Author { get; set; } = null!;
    public ICollection<ArticleVersion> Versions { get; set; } = [];
    public ICollection<ArticleTag> Tags { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<ArticleReference> References { get; set; } = [];
    public ICollection<RelatedArticle> RelatedArticles { get; set; } = [];
}
