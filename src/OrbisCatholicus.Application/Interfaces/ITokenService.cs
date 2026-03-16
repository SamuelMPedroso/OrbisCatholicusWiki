using OrbisCatholicus.Application.DTOs;

namespace OrbisCatholicus.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(int userId, string email, string username, string accessLevel);
    string GenerateRefreshToken();
    int? GetUserIdFromExpiredToken(string token);
}
