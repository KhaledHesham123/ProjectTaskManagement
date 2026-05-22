using FluentValidation;
using ProjectTaskManagement.Application.Common.Validation;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(300).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(4000).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage(ValidationMessages.InvalidGuid);
    }
}
