using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskManagement.Application.Features.Projects.Commands.CreateProject;
using ProjectTaskManagement.Application.Features.Projects.Commands.DeleteProject;
using ProjectTaskManagement.Application.Features.Projects.Commands.UpdateProject;
using ProjectTaskManagement.Application.Features.Projects.Queries.GetProjectById;
using ProjectTaskManagement.Application.Features.Projects.Queries.GetProjects;
using ProjectTaskManagement.Infrastructure.DynamicRBASystem;
using static ProjectTaskManagement.Domain.Common.ApplicationConstants;

namespace ProjectTaskManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController(ISender sender) : ControllerBase
{
    [HttpPost("Create")]
    [HasPermission(AppPermissions.Create)]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpGet("GetAll")]
    [HasPermission(AppPermissions.View)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProjectsQuery(), cancellationToken);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpGet("GetById")]
    [HasPermission(AppPermissions.View)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProjectByIdQuery(id), cancellationToken);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    [HasPermission(AppPermissions.Edit)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UpdateProjectCommand(id, request.Name, request.Description),
            cancellationToken);

        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    [HttpDelete]
    [HasPermission(AppPermissions.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteProjectCommand(id), cancellationToken);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
}

public record UpdateProjectRequest(string Name, string? Description);
