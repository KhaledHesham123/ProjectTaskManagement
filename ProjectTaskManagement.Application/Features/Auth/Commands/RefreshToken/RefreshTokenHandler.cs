using MediatR;
using ProjectTaskManagement.Application.Common.Exceptions;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Auth.Dtos;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenHandler(ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, AuthTokensDto>
{
    public async Task<AuthTokensDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = await tokenService.ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedException("Invalid or expired refresh token.");

        throw new NotImplementedException("Load user and call GenerateTokensAsync after validating refresh token.");
    }
}
