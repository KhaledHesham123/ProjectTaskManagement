using MediatR;
using ProjectTaskManagement.Application.Common.Exceptions;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Projects.Dtos;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Projects.Queries.GetProjectById;

public class GetProjectByIdHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{
    public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await unitOfWork.Repository<Project>().FindAsync(
            p => p.Id == request.Id,
            asNoTracking: true,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Project), request.Id);

        return new ProjectDto(
            project.Id,
            project.Name,
            project.Description,
            project.CreatedAt);
    }
}
