using MediatR;
using ProjectTaskManagement.Application.Features.Auth.Dtos;
using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password)
    : ICommand<Result<TokenDto>>;
