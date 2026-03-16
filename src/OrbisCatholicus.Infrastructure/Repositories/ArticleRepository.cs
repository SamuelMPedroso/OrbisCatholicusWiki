using Microsoft.EntityFrameworkCore;
using OrbisCatholicus.Domain.Entities;
using OrbisCatholicus.Domain.Enums;
using OrbisCatholicus.Domain.Interfaces;
using OrbisCatholicus.Infrastructure.Data;

namespace OrbisCatholicus.Infrastructure.Repositories;

public class ArticleRepository : Repository<Article>, IArticleRepository
{
    public ArticleRepository(AppDbContext context) : base(context) { }

    public async Task<Article?> GetBySlugAsync(string slug)
    {
        return await _dbSet
            .Include(a => a.Category)
            .Include(a => a.Author)
            .Include(a => a.CurrentVersion!)
                .ThenInclude(v => v.ReviewStatus)
            .Include(a => a.Tags)
            .Include(a => a.References.OrderBy(r => r.DisplayOrder))
            .Include(a => a.RelatedArticles)
                .ThenInclude(ra => ra.Related)
            .FirstOrDefaultAsync(a => a.Slug == slug);
    }

    public async Task<IEnumerable<Article>> GetFeaturedAsync(int count = 5)
    {
        return await _dbSet
            .Where(a => a.IsFeatured)
            .Include(a => a.Category)
            .Include(a => a.Author)
            .Include(a => a.CurrentVersion!)
                .ThenInclude(v => v.ReviewStatus)
            .OrderByDescending(a => a.UpdatedAt ?? a.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetRecentAsync(int count = 10)
    {
        return await _dbSet
            .Include(a => a.Category)
            .Include(a => a.Author)
            .Where(a => a.CurrentVersion != null &&
                        a.CurrentVersion.ReviewStatusId == (int)ReviewStatusEnum.Aprovado)
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetPopularAsync(int count = 10)
    {
        return await _dbSet
            .Include(a => a.Category)
            .Include(a => a.Author)
            .Where(a => a.CurrentVersion != null &&
                        a.CurrentVersion.ReviewStatusId == (int)ReviewStatusEnum.Aprovado)
            .OrderByDescending(a => a.Views)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetByCategorySlugAsync(string categorySlug, int page, int pageSize)
    {
        return await _dbSet
            .Include(a => a.Category)
            .Include(a => a.Author)
            .Where(a => a.Category != null && a.Category.Slug == categorySlug)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> SearchAsync(string term, int page, int pageSize)
    {
        var lowerTerm = term.ToLower();
        return await _dbSet
            .Include(a => a.Category)
            .Include(a => a.Author)
            .Where(a => EF.Functions.ILike(a.Title, $"%{lowerTerm}%") ||
                        (a.Summary != null && EF.Functions.ILike(a.Summary, $"%{lowerTerm}%")))
            .OrderByDescending(a => a.Views)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> SearchCountAsync(string term)
    {
        var lowerTerm = term.ToLower();
        return await _dbSet
            .CountAsync(a => EF.Functions.ILike(a.Title, $"%{lowerTerm}%") ||
                             (a.Summary != null && EF.Functions.ILike(a.Summary, $"%{lowerTerm}%")));
    }

    public async Task IncrementViewsAsync(int articleId)
    {
        await _context.Articles
            .Where(a => a.Id == articleId)
            .ExecuteUpdateAsync(s => s.SetProperty(a => a.Views, a => a.Views + 1));
    }
}
