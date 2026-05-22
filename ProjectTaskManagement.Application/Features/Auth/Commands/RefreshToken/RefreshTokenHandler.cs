using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Auth.Dtos;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities.Identity;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenHandler(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, Result<TokenDto>>
{
    public async Task<Result<TokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = await tokenService.ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (string.IsNullOrEmpty(userId))
            return Result<TokenDto>.Fail("Invalid or expired refresh token.");

        var user = await userManager.Users
            .AsNoTracking()
            .Where(x => x.Id == userId && !x.Is_Deleted)
            .Select(x => new UserTokenProjection(
                x.Id,
                x.UserName ?? string.Empty,
                x.Email ?? string.Empty,
                x.Is_Active,
                x.UserRoles
                    .Where(ur => ur.Role != null && ur.Role.Name != null)
                    .Select(ur => ur.Role!.Name!)
                    .ToList(),
                x.UserPermissions
                    .Where(up => !up.IsDeleted)
                    .Select(up => up.Permission)
                    .Distinct()
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result<TokenDto>.Fail("User not found.");

        var token = await tokenService.GenerateTokenAsync(user, cancellationToken);
        return Result<TokenDto>.Success(token);
    }
}
