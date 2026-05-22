namespace ProjectTaskManagement.Application.Features.Auth.Dtos;

public record AuthResponseDto(
    string UserId,
    string UserName,
    string Email,
    AuthTokensDto Tokens);
