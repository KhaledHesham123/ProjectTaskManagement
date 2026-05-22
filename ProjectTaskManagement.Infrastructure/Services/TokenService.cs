using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Auth.Dtos;
using ProjectTaskManagement.Domain.Entities.Auth;
using ProjectTaskManagement.Infrastructure.Settings;

namespace ProjectTaskManagement.Infrastructure.Services;

public class TokenService(
    IOptions<JwtSettings> jwtOptions,
    IGenericRepository<RefreshToken> refreshTokenRepository,
    IUnitOfWork unitOfWork) : ITokenService
{
    private readonly JwtSettings _settings = jwtOptions.Value;

    public async Task<TokenDto> GenerateTokenAsync(
        UserTokenProjection user,
        CancellationToken cancellationToken = default)
    {
        var accessTokenExpires = DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes);
        var refreshTokenExpires = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays);

        var refreshTokenValue = GenerateRefreshToken();
        var accessToken = CreateAccessToken(
            user.Id,
            user.UserName,
            user.Roles,
            user.Permissions,
            accessTokenExpires);

        var existingTokens = await refreshTokenRepository
            .GetByCriteriaQueryable(t => t.User_Id == user.Id)
            .ToListAsync(cancellationToken);

        foreach (var existingToken in existingTokens)
            refreshTokenRepository.Delete(existingToken);

        await refreshTokenRepository.AddAsync(
            new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshTokenValue,
                Expires_On = refreshTokenExpires,
                User_Id = user.Id
            },
            cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new TokenDto(accessToken, refreshTokenValue, accessTokenExpires, refreshTokenExpires);
    }

    public async Task<string?> ValidateRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default) =>
        await refreshTokenRepository
            .GetByCriteriaQueryable(t => t.Token == refreshToken && t.Expires_On > DateTime.UtcNow)
            .Select(t => t.User_Id)
            .FirstOrDefaultAsync(cancellationToken);

    private string CreateAccessToken(
        string userId,
        string userName,
        IEnumerable<string> roles,
        IEnumerable<string> permissions,
        DateTime expires)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, userName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        claims.AddRange(permissions.Select(p => new Claim("Permission", p)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
