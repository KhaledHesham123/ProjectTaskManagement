using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskManagement.Application.Features.Auth.Commands.Login;
using ProjectTaskManagement.Application.Features.Auth.Commands.RefreshToken;
using ProjectTaskManagement.Application.Features.Auth.Commands.Register;

namespace ProjectTaskManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return Ok(result);
    }
}
