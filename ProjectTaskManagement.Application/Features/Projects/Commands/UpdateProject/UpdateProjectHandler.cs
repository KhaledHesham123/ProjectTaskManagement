using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Projects.Dtos;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectHandler(
    IGenericRepository<Project> projectRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProjectCommand, Result<ProjectDto>>
{
    public async Task<Result<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository
            .GetByCriteriaQueryable(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (project is null)
            return Result<ProjectDto>.Fail("Project not found.");

        project.Name = request.Name;
        project.Description = request.Description;

        projectRepository.Update(project);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ProjectDto>.Success(
            new ProjectDto(
                project.Id,
                project.Name,
                project.Description,
                project.CreatedAt));
    }
}
