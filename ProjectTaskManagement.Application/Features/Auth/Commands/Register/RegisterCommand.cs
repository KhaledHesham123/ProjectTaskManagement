using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string UserName,
    string Email,
    string Password) : ICommand<Result<bool>>;
