using ProjectTaskManagement.Application.Features.Auth.Dtos;

namespace ProjectTaskManagement.Application.Common.Interfaces;

public interface ITokenService
{
    Task<TokenDto> GenerateTokenAsync(
        UserTokenProjection user,
        CancellationToken cancellationToken = default);

    Task<string?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
