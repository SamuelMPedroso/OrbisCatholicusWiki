using Microsoft.EntityFrameworkCore;
using OrbisCatholicus.Domain.Entities;
using OrbisCatholicus.Domain.Interfaces;
using OrbisCatholicus.Infrastructure.Data;

namespace OrbisCatholicus.Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) { }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _dbSet
            .Include(c => c.SubCategories)
            .Include(c => c.ParentCategory)
            .FirstOrDefaultAsync(c => c.Slug == slug);
    }

    public async Task<IEnumerable<Category>> GetRootCategoriesAsync()
    {
        return await _dbSet
            .Where(c => c.ParentCategoryId == null)
            .OrderBy(c => c.DisplayOrder)
            .Include(c => c.SubCategories)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetWithArticleCountAsync()
    {
        return await _dbSet
            .Where(c => c.ParentCategoryId == null)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                IconClass = c.IconClass,
                DisplayOrder = c.DisplayOrder,
                Articles = c.Articles
            })
            .ToListAsync();
    }
}
