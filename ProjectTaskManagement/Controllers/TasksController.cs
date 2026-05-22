using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskManagement.Application.Features.Tasks.Commands.CreateTask;
using ProjectTaskManagement.Application.Features.Tasks.Queries.GetTasksByProject;

namespace ProjectTaskManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByProject), new { projectId = result.ProjectId }, result);
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> GetByProject(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTasksByProjectQuery(projectId), cancellationToken);
        return Ok(result);
    }
}
