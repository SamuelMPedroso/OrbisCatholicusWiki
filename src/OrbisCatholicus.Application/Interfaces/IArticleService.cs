using OrbisCatholicus.Application.Common;
using OrbisCatholicus.Application.DTOs;

namespace OrbisCatholicus.Application.Interfaces;

public interface IArticleService
{
    Task<Result<ArticleDetailDto>> GetBySlugAsync(string slug);
    Task<Result<IEnumerable<ArticleListDto>>> GetFeaturedAsync(int count = 5);
    Task<Result<IEnumerable<ArticleListDto>>> GetRecentAsync(int count = 10);
    Task<Result<IEnumerable<ArticleListDto>>> GetPopularAsync(int count = 10);
    Task<Result<SearchResultDto>> SearchAsync(string query, int page = 1, int pageSize = 10);
    Task<Result<ArticleDetailDto>> CreateAsync(CreateArticleDto dto, int userId);
    Task<Result> UpdateAsync(int id, UpdateArticleDto dto, int userId);
    Task<Result> DeactivateAsync(int id, int userId);
}
