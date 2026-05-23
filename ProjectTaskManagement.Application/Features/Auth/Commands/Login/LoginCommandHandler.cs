using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Auth.Dtos;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities.Identity;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService)
    : IRequestHandler<LoginCommand, Result<TokenDto>>
{
    public async Task<Result<TokenDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result<TokenDto>.Fail("Invalid login credentials. Check your email.");

        if (!user.Is_Active)
            return Result<TokenDto>.Fail("Your account is inactive. Please contact support.");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
            return Result<TokenDto>.Fail("Invalid login credentials. Check your password.");

        var userProjection = await userManager.Users
            .AsNoTracking()
            .Where(x => x.Id == user.Id)
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
            .FirstAsync(cancellationToken);

        var token = await tokenService.GenerateTokenAsync(userProjection, cancellationToken);

        return Result<TokenDto>.Success(token);
    }
}
