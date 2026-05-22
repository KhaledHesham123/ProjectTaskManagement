using FluentValidation;
using ProjectTaskManagement.Domain.Enums;

namespace ProjectTaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusValidator : AbstractValidator<UpdateTaskStatusCommand>
{
    public UpdateTaskStatusValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Task id is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status is not valid.");
    }
}
