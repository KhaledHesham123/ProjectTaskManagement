using FluentValidation;

namespace ProjectTaskManagement.Application.Features.Projects.Commands.DeleteProject;

public class DeleteProjectValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Project id is required.");
    }
}
