using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OrbisCatholicus.Application.Common;
using OrbisCatholicus.Application.DTOs;
using OrbisCatholicus.Application.Interfaces;
using OrbisCatholicus.Domain.Entities;
using OrbisCatholicus.Domain.Enums;
using OrbisCatholicus.Domain.Interfaces;

namespace OrbisCatholicus.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(IUnitOfWork uow, IMapper mapper, ITokenService tokenService, IPasswordHasher<User> passwordHasher)
    {
        _uow = uow;
        _mapper = mapper;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        if (await _uow.Users.ExistsAsync(u => u.Email == dto.Email))
            return Result<AuthResponseDto>.Fail("Email já cadastrado.");

        if (await _uow.Users.ExistsAsync(u => u.Username == dto.Username))
            return Result<AuthResponseDto>.Fail("Nome de usuário já existe.");

        var user = new User
        {
            Username = dto.Username,
            DisplayName = dto.DisplayName,
            Email = dto.Email,
            AccessLevelId = (int)AccessLevelEnum.Leitor
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _uow.Users.AddAsync(user);
        await _uow.SaveChangesAsync();

        // Reload to get AccessLevel
        var created = await _uow.Users.GetByEmailAsync(dto.Email);

        var accessToken = _tokenService.GenerateAccessToken(
            created!.Id, created.Email, created.Username, created.AccessLevel.Name);

        return Result<AuthResponseDto>.Ok(new AuthResponseDto(
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            Expiration: DateTime.UtcNow.AddHours(2),
            User: _mapper.Map<UserProfileDto>(created)
        ));
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _uow.Users.GetByEmailAsync(dto.Email);
        if (user == null)
            return Result<AuthResponseDto>.Fail("Credenciais inválidas.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed)
            return Result<AuthResponseDto>.Fail("Credenciais inválidas.");

        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        user.LastLoginAt = DateTime.UtcNow;

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();

        var accessToken = _tokenService.GenerateAccessToken(
            user.Id, user.Email, user.Username, user.AccessLevel.Name);

        return Result<AuthResponseDto>.Ok(new AuthResponseDto(
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            Expiration: DateTime.UtcNow.AddHours(2),
            User: _mapper.Map<UserProfileDto>(user)
        ));
    }

    public async Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var userId = _tokenService.GetUserIdFromExpiredToken(dto.AccessToken);
        if (userId == null)
            return Result<AuthResponseDto>.Fail("Token inválido.");

        var user = await _uow.Users.GetByRefreshTokenAsync(dto.RefreshToken);
        if (user == null || user.Id != userId || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return Result<AuthResponseDto>.Fail("Refresh token inválido ou expirado.");

        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();

        var accessToken = _tokenService.GenerateAccessToken(
            user.Id, user.Email, user.Username, user.AccessLevel.Name);

        return Result<AuthResponseDto>.Ok(new AuthResponseDto(
            AccessToken: accessToken,
            RefreshToken: newRefreshToken,
            Expiration: DateTime.UtcNow.AddHours(2),
            User: _mapper.Map<UserProfileDto>(user)
        ));
    }

    public async Task<Result<UserProfileDto>> GetProfileAsync(int userId)
    {
        var user = await _uow.Users.GetByIdAsync(userId);
        if (user == null)
            return Result<UserProfileDto>.Fail("Usuário não encontrado.");

        return Result<UserProfileDto>.Ok(_mapper.Map<UserProfileDto>(user));
    }
}
