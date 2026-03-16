using OrbisCatholicus.Application.Common;
using OrbisCatholicus.Application.DTOs;

namespace OrbisCatholicus.Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);
    Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto);
    Task<Result<UserProfileDto>> GetProfileAsync(int userId);
}
