using Microsoft.EntityFrameworkCore;
using OrbisCatholicus.Domain.Entities;
using OrbisCatholicus.Domain.Interfaces;
using OrbisCatholicus.Infrastructure.Data;

namespace OrbisCatholicus.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.AccessLevel)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .Include(u => u.AccessLevel)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _dbSet
            .Include(u => u.AccessLevel)
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.IsActive);
    }
}
