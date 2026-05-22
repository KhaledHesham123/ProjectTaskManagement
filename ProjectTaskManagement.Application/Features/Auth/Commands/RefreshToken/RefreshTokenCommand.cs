using ProjectTaskManagement.Application.Features.Auth.Dtos;
using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<Result<TokenDto>>;
