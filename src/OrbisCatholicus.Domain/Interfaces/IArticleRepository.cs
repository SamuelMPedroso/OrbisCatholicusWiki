using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Domain.Interfaces;

public interface IArticleRepository : IRepository<Article>
{
    Task<Article?> GetBySlugAsync(string slug);
    Task<IEnumerable<Article>> GetFeaturedAsync(int count = 5);
    Task<IEnumerable<Article>> GetRecentAsync(int count = 10);
    Task<IEnumerable<Article>> GetPopularAsync(int count = 10);
    Task<IEnumerable<Article>> GetByCategorySlugAsync(string categorySlug, int page, int pageSize);
    Task<IEnumerable<Article>> SearchAsync(string term, int page, int pageSize);
    Task<int> SearchCountAsync(string term);
    Task IncrementViewsAsync(int articleId);
}
