using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Projects.Commands.DeleteProject;

public class DeleteProjectHandler(
    IGenericRepository<Project> projectRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteProjectCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository
            .GetByCriteriaQueryable(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (project is null)
            return Result<bool>.Fail("Project not found.");

        projectRepository.Delete(project);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
