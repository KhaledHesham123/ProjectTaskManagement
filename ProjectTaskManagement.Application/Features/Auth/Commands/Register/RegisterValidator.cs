using FluentValidation;
using ProjectTaskManagement.Application.Common.Validation;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(100).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .EmailAddress().WithMessage(ValidationMessages.InvalidEmail)
            .MaximumLength(256).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MinimumLength(8).WithMessage(ValidationMessages.MinLength)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
    }
}
