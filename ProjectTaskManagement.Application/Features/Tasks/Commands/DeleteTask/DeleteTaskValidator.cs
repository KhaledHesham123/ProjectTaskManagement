using FluentValidation;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Task id is required.");
    }
}
