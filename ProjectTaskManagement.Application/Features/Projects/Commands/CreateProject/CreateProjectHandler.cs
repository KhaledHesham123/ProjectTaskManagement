using MediatR;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectHandler(
    IGenericRepository<Project> projectRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateProjectCommand, bool>
{
    public async Task<bool> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        await projectRepository.AddAsync(project, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
