using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskManagement.Application.Features.Tasks.Commands.CreateTask;
using ProjectTaskManagement.Application.Features.Tasks.Commands.DeleteTask;
using ProjectTaskManagement.Application.Features.Tasks.Commands.UpdateTaskStatus;
using ProjectTaskManagement.Application.Features.Tasks.Queries.GetTasksByProject;
using ProjectTaskManagement.Domain.Enums;
using ProjectTaskManagement.Extensions;
using ProjectTaskManagement.Infrastructure.DynamicRBASystem;
using static ProjectTaskManagement.Domain.Common.ApplicationConstants;

namespace ProjectTaskManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(ISender sender) : ControllerBase
{
    [HttpPost("Create")]
    [HasPermission(AppPermissions.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var succeeded = await sender.Send(command, cancellationToken);
        return succeeded ? Ok(true) : BadRequest(false);
    }

    [HttpPatch("UpdateStatus")]
    [HasPermission(AppPermissions.Edit)]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateTaskStatusRequest request,
        CancellationToken cancellationToken)
    {
        var succeeded = await sender.Send(
            new UpdateTaskStatusCommand(id, request.Status),
            cancellationToken);

        return succeeded ? Ok(true) : BadRequest(false);
    }

    [HttpGet("GetTasksByProjectID")]
    [HasPermission(AppPermissions.View)]
    public async Task<IActionResult> GetByProject(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTasksByProjectQuery(projectId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("Delete")]
    [HasPermission(AppPermissions.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteTaskCommand(id), cancellationToken);
        return result.ToActionResult();
    }
}

public record UpdateTaskStatusRequest(TaskItemStatus Status);
