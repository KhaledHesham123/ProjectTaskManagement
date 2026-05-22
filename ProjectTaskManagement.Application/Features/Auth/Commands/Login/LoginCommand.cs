using MediatR;
using ProjectTaskManagement.Application.Features.Auth.Dtos;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
