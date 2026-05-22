using ProjectTaskManagement.Application.Features.Auth.Dtos;

namespace ProjectTaskManagement.Application.Common.Interfaces;

public interface ITokenService
{
    Task<AuthTokensDto> GenerateTokensAsync(
        string userId,
        string userName,
        IEnumerable<string> roles,
        IEnumerable<string> permissions,
        CancellationToken cancellationToken = default);

    Task<string?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
