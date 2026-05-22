namespace ProjectTaskManagement.Application.Features.Auth.Dtos;

public record AuthTokensDto(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt,
    DateTime RefreshTokenExpiresAt);
