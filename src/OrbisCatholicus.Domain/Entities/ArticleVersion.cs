namespace OrbisCatholicus.Domain.Entities;

public class ArticleVersion : BaseEntity
{
    public int ArticleId { get; set; }
    public int VersionNumber { get; set; } = 1;
    public string Content { get; set; } = string.Empty;
    public string ContentHtml { get; set; } = string.Empty;
    public string? EditSummary { get; set; }
    public int CreatedBy { get; set; }
    public int? ReviewStatusId { get; set; }
    public int? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? DeactivatedAt { get; set; }

    // Navigation
    public Article Article { get; set; } = null!;
    public User Author { get; set; } = null!;
    public User? Reviewer { get; set; }
    public ReviewStatus? ReviewStatus { get; set; }
    public ICollection<ArticleReviewHistory> ReviewHistories { get; set; } = [];
}
