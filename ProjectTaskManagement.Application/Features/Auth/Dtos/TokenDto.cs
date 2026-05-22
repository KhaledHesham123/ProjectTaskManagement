namespace ProjectTaskManagement.Application.Features.Auth.Dtos;

public record TokenDto(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt,
    DateTime RefreshTokenExpiresAt);
