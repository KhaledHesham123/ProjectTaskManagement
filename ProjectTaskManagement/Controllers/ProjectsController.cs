using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskManagement.Application.Features.Projects.Commands.CreateProject;
using ProjectTaskManagement.Application.Features.Projects.Queries.GetProjectById;
using ProjectTaskManagement.Application.Features.Projects.Queries.GetProjects;

namespace ProjectTaskManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProjectsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProjectByIdQuery(id), cancellationToken);
        return Ok(result);
    }
}
