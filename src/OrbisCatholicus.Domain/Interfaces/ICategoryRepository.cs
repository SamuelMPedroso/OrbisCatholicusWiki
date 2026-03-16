using OrbisCatholicus.Domain.Entities;

namespace OrbisCatholicus.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetBySlugAsync(string slug);
    Task<IEnumerable<Category>> GetRootCategoriesAsync();
    Task<IEnumerable<Category>> GetWithArticleCountAsync();
}
