using MediatR;
using Microsoft.AspNetCore.Identity;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities.Identity;
using static ProjectTaskManagement.Domain.Common.ApplicationConstants;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.Register;

public class RegisterHandler(
    UserManager<ApplicationUser> userManager,
    IGenericRepository<UserPermission> userPermissionRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingByEmail = await userManager.FindByEmailAsync(request.Email);
        var existingByUserName = await userManager.FindByNameAsync(request.UserName);

        if (existingByEmail is not null || existingByUserName is not null)
            return Result<bool>.Fail("A user with this email or username already exists.");

        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            EmailConfirmed = true,
            Is_Active = true
        };

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            return Result<bool>.Fail(
                createResult.Errors.Select(e => e.Description).ToList());
        }

        await userManager.AddToRoleAsync(user, nameof(UserRole.User));

        await userPermissionRepository.AddAsync(
            new UserPermission
            {
                Id = Guid.NewGuid(),
                User_Id = user.Id,
                Permission = AppPermissions.View
            },
            cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, "User registered successfully.");
    }
}
