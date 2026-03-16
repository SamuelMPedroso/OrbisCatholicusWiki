using System.Collections.Concurrent;
using OrbisCatholicus.Domain.Interfaces;
using OrbisCatholicus.Infrastructure.Data;

namespace OrbisCatholicus.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    private IArticleRepository? _articles;
    private IArticleVersionRepository? _articleVersions;
    private ICategoryRepository? _categories;
    private IUserRepository? _users;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IArticleRepository Articles =>
        _articles ??= new ArticleRepository(_context);

    public IArticleVersionRepository ArticleVersions =>
        _articleVersions ??= new ArticleVersionRepository(_context);

    public ICategoryRepository Categories =>
        _categories ??= new CategoryRepository(_context);

    public IUserRepository Users =>
        _users ??= new UserRepository(_context);

    public IRepository<T> Repository<T>() where T : class
    {
        return (IRepository<T>)_repositories.GetOrAdd(typeof(T), _ => new Repository<T>(_context));
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
