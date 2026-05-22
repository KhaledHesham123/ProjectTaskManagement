using FluentValidation;
using ProjectTaskManagement.Application.Common.Validation;

namespace ProjectTaskManagement.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage(ValidationMessages.MaxLength);
    }
}
