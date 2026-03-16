using Microsoft.EntityFrameworkCore;
using OrbisCatholicus.Domain.Entities;
using OrbisCatholicus.Domain.Enums;
using OrbisCatholicus.Domain.Interfaces;
using OrbisCatholicus.Infrastructure.Data;

namespace OrbisCatholicus.Infrastructure.Repositories;

public class ArticleVersionRepository : Repository<ArticleVersion>, IArticleVersionRepository
{
    public ArticleVersionRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<ArticleVersion>> GetByArticleIdAsync(int articleId)
    {
        return await _dbSet
            .Where(v => v.ArticleId == articleId)
            .Include(v => v.Author)
            .Include(v => v.ReviewStatus)
            .OrderByDescending(v => v.VersionNumber)
            .ToListAsync();
    }

    public async Task<int> GetNextVersionNumberAsync(int articleId)
    {
        var max = await _dbSet
            .Where(v => v.ArticleId == articleId)
            .MaxAsync(v => (int?)v.VersionNumber) ?? 0;
        return max + 1;
    }

    public async Task<IEnumerable<ArticleVersion>> GetPendingReviewsAsync(int page, int pageSize)
    {
        return await _dbSet
            .Where(v => v.ReviewStatusId == (int)ReviewStatusEnum.Pendente ||
                        v.ReviewStatusId == (int)ReviewStatusEnum.RevisaoSolicitada)
            .Include(v => v.Article)
            .Include(v => v.Author)
            .Include(v => v.ReviewStatus)
            .OrderBy(v => v.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
