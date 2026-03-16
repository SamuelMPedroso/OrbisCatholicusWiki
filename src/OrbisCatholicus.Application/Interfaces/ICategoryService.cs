using OrbisCatholicus.Application.Common;
using OrbisCatholicus.Application.DTOs;

namespace OrbisCatholicus.Application.Interfaces;

public interface ICategoryService
{
    Task<Result<IEnumerable<CategoryListDto>>> GetAllAsync();
    Task<Result<CategoryDetailDto>> GetBySlugAsync(string slug);
    Task<Result<PagedResult<ArticleListDto>>> GetArticlesByCategoryAsync(string slug, int page = 1, int pageSize = 20);
}
