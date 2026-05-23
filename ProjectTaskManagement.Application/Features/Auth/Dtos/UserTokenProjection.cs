namespace ProjectTaskManagement.Application.Features.Auth.Dtos;



public record UserTokenProjection(

    string Id,

    string UserName,

    string Email,

    bool Is_Active,

    IReadOnlyList<string> Roles,

    IReadOnlyList<string> Permissions);

