using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Projects.Dtos;
using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Projects.Queries.GetProjects;

public class GetProjectsHandler(IGenericRepository<Project> projectRepository)
    : IRequestHandler<GetProjectsQuery, Result<IReadOnlyList<ProjectDto>>>
{
    public async Task<Result<IReadOnlyList<ProjectDto>>> Handle(
        GetProjectsQuery request,
        CancellationToken cancellationToken)
    {
        var projects = await projectRepository
            .GetAll()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);

        var data = projects
            .Select(p => new ProjectDto(p.Id, p.Name, p.Description, p.CreatedAt))
            .ToList();

        return Result<IReadOnlyList<ProjectDto>>.Success(data);
    }
}
