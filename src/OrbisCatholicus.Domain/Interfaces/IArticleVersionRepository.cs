using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Domain.Interfaces;

public interface IArticleVersionRepository : IRepository<ArticleVersion>
{
    Task<IEnumerable<ArticleVersion>> GetByArticleIdAsync(int articleId);
    Task<int> GetNextVersionNumberAsync(int articleId);
    Task<IEnumerable<ArticleVersion>> GetPendingReviewsAsync(int page, int pageSize);
}
