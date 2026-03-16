namespace OrbisCatholicus.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IArticleRepository Articles { get; }
    IArticleVersionRepository ArticleVersions { get; }
    ICategoryRepository Categories { get; }
    IUserRepository Users { get; }
    IRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync();
}
