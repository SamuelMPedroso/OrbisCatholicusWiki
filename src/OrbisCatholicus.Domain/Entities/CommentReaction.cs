namespace OrbisCatholicus.Domain.Entities;

public class CommentReaction
{
    public int Id { get; set; }
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public string ReactionType { get; set; } = "like";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Comment Comment { get; set; } = null!;
    public User User { get; set; } = null!;
}
