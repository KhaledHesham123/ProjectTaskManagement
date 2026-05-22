using MediatR;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Features.Projects.Dtos;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        await unitOfWork.Repository<Project>().AddAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ProjectDto(
            project.Id,
            project.Name,
            project.Description,
            project.CreatedAt);
    }
}
