namespace OrbisCatholicus.Application.DTOs;

public record RegisterDto(
    string Username,
    string DisplayName,
    string Email,
    string Password
);

public record LoginDto(
    string Email,
    string Password
);

public record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime Expiration,
    UserProfileDto User
);

public record RefreshTokenDto(
    string AccessToken,
    string RefreshToken
);

public record UserProfileDto(
    int Id,
    string Username,
    string DisplayName,
    string Email,
    string? AvatarUrl,
    string? Bio,
    string AccessLevelName
);
