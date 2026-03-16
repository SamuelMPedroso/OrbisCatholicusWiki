using Microsoft.EntityFrameworkCore;
using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AccessLevel> AccessLevels => Set<AccessLevel>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<ArticleVersion> ArticleVersions => Set<ArticleVersion>();
    public DbSet<ReviewStatus> ReviewStatuses => Set<ReviewStatus>();
    public DbSet<ArticleReviewHistory> ArticleReviewHistories => Set<ArticleReviewHistory>();
    public DbSet<ArticleTag> ArticleTags => Set<ArticleTag>();
    public DbSet<ArticleReference> ArticleReferences => Set<ArticleReference>();
    public DbSet<RelatedArticle> RelatedArticles => Set<RelatedArticle>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<CommentReaction> CommentReactions => Set<CommentReaction>();
    public DbSet<UserFavorite> UserFavorites => Set<UserFavorite>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Global Query Filters (Soft Delete)
        modelBuilder.Entity<Article>().HasQueryFilter(a => a.IsActive);
        modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);
        modelBuilder.Entity<Category>().HasQueryFilter(c => c.IsActive);
        modelBuilder.Entity<Comment>().HasQueryFilter(c => c.IsActive);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
