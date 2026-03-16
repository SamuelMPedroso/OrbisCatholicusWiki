namespace OrbisCatholicus.Domain.Entities;

public class ArticleReviewHistory : BaseEntity
{
    public int ArticleVersionId { get; set; }
    public int ReviewerId { get; set; }
    public int? PreviousStatusId { get; set; }
    public int NewStatusId { get; set; }
    public string? Notes { get; set; }
    public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ArticleVersion ArticleVersion { get; set; } = null!;
    public User Reviewer { get; set; } = null!;
    public ReviewStatus? PreviousStatus { get; set; }
    public ReviewStatus NewStatus { get; set; } = null!;
}
