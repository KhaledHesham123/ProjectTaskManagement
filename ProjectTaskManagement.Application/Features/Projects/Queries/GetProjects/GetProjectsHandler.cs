using MediatR;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Projects.Dtos;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Projects.Queries.GetProjects;

public class GetProjectsHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProjectsQuery, IReadOnlyList<ProjectDto>>
{
    public async Task<IReadOnlyList<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await unitOfWork.Repository<Project>().GetAllAsync(
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            cancellationToken: cancellationToken);

        return projects
            .Select(p => new ProjectDto(p.Id, p.Name, p.Description, p.CreatedAt))
            .ToList();
    }
}
