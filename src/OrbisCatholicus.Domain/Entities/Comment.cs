namespace OrbisCatholicus.Domain.Entities;

public class Comment : SoftDeletableEntity
{
    public int ArticleId { get; set; }
    public int UserId { get; set; }
    public int? ParentCommentId { get; set; }
    public string Content { get; set; } = string.Empty;

    // Navigation
    public Article Article { get; set; } = null!;
    public User User { get; set; } = null!;
    public Comment? ParentComment { get; set; }
    public ICollection<Comment> Replies { get; set; } = [];
    public ICollection<CommentReaction> Reactions { get; set; } = [];
}
