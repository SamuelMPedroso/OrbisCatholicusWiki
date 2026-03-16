namespace OrbisCatholicus.Domain.Entities;

public class UserFavorite
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ArticleId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public Article Article { get; set; } = null!;
}
