using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Auth.Dtos;
using ProjectTaskManagement.Domain.Entities.Identity;
using ProjectTaskManagement.Infrastructure.Persistence;
using ProjectTaskManagement.Infrastructure.Settings;

namespace ProjectTaskManagement.Infrastructure.Services;

public class TokenService(
    IOptions<JwtSettings> jwtOptions,
    UserManager<ApplicationUser> userManager,
    AppDbContext dbContext) : ITokenService
{
    private readonly JwtSettings _settings = jwtOptions.Value;

    public async Task<AuthTokensDto> GenerateTokensAsync(
        string userId,
        string userName,
        IEnumerable<string> roles,
        IEnumerable<string> permissions,
        CancellationToken cancellationToken = default)
    {
        var accessTokenExpires = DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes);
        var refreshTokenExpires = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays);

        var refreshToken = GenerateRefreshToken();
        var accessToken = CreateAccessToken(userId, userName, roles, permissions, accessTokenExpires);

        var user = await userManager.FindByIdAsync(userId);
        if (user is not null)
        {
            user.Refresh_Token = refreshToken;
            user.Refresh_Token_Expires_At = refreshTokenExpires;
            await userManager.UpdateAsync(user);
        }

        return new AuthTokensDto(accessToken, refreshToken, accessTokenExpires, refreshTokenExpires);
    }

    public async Task<string?> ValidateRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u =>
                u.Refresh_Token == refreshToken &&
                u.Refresh_Token_Expires_At > DateTime.UtcNow &&
                !u.Is_Deleted,
                cancellationToken);

        return user?.Id;
    }

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
        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

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
