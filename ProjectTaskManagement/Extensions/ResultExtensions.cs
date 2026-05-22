using Microsoft.AspNetCore.Mvc;
using ProjectTaskManagement.Domain.Common;

namespace ProjectTaskManagement.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result) =>
        result.Succeeded ? new OkObjectResult(result) : new BadRequestObjectResult(result);

    public static IActionResult ToCreatedResult<T>(
        this Result<T> result,
        ControllerBase controller,
        string actionName,
        object routeValues) =>
        result.Succeeded
            ? controller.CreatedAtAction(actionName, routeValues, result)
            : new BadRequestObjectResult(result);
}
