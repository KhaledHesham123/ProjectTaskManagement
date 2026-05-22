using System.Net;
using System.Text.Json;
using ProjectTaskManagement.Application.Common.Exceptions;
using ProjectTaskManagement.Application.Common.Models;

namespace ProjectTaskManagement.Middleware;

public class GlobalExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, response) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                ApiResponse<object>.Fail(
                    "Validation failed.",
                    validationEx.Errors.SelectMany(e => e.Value))),

            NotFoundException notFoundEx => (
                HttpStatusCode.NotFound,
                ApiResponse<object>.Fail(notFoundEx.Message)),

            UnauthorizedException unauthorizedEx => (
                HttpStatusCode.Unauthorized,
                ApiResponse<object>.Fail(unauthorizedEx.Message)),

            ForbiddenException forbiddenEx => (
                HttpStatusCode.Forbidden,
                ApiResponse<object>.Fail(forbiddenEx.Message)),

            _ => (
                HttpStatusCode.InternalServerError,
                ApiResponse<object>.Fail("An unexpected error occurred."))
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
