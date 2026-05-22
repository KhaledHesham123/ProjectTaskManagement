using MediatR;
using ProjectTaskManagement.Application.Features.Auth.Dtos;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthTokensDto>;
