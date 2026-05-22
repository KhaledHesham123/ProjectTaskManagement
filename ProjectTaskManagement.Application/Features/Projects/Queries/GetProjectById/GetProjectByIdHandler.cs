using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Projects.Dtos;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Projects.Queries.GetProjectById;

public class GetProjectByIdHandler(IGenericRepository<Project> projectRepository)
    : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
{
    public async Task<Result<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository
            .GetByCriteriaQueryable(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (project is null)
            return Result<ProjectDto>.Fail("Project not found.");

        return Result<ProjectDto>.Success(
            new ProjectDto(
                project.Id,
                project.Name,
                project.Description,
                project.CreatedAt));
    }
}
