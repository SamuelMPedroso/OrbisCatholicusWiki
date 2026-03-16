namespace OrbisCatholicus.Domain.Entities;

public class User : SoftDeletableEntity
{
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public int AccessLevelId { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navigation
    public AccessLevel AccessLevel { get; set; } = null!;
    public ICollection<Article> Articles { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<UserFavorite> Favorites { get; set; } = [];
}
