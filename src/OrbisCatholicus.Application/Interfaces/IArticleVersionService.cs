using OrbisCatholicus.Application.Common;
using OrbisCatholicus.Application.DTOs;

namespace OrbisCatholicus.Application.Interfaces;

public interface IArticleVersionService
{
    Task<Result<IEnumerable<ArticleVersionListDto>>> GetByArticleIdAsync(int articleId);
    Task<Result<ArticleVersionListDto>> CreateVersionAsync(int articleId, CreateVersionDto dto, int userId);
    Task<Result> ReviewAsync(int versionId, ReviewVersionDto dto, int reviewerId);
    Task<Result<IEnumerable<ArticleVersionListDto>>> GetPendingReviewsAsync(int page = 1, int pageSize = 20);
}
