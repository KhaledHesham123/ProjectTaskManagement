namespace ProjectTaskManagement.Domain.Common;

public class Result<T> : IResult
{
    public bool Succeeded { get; set; }
    public List<string> Messages { get; set; } = [];
    public T? Data { get; set; }

    public static Result<T> Success() =>
        new() { Succeeded = true };

    public static Result<T> Success(T data) =>
        new() { Succeeded = true, Data = data };

    public static Result<T> Success(string message) =>
        new() { Succeeded = true, Messages = [message] };

    public static Result<T> Success(T data, string message) =>
        new()
        {
            Succeeded = true,
            Data = data,
            Messages = [message]
        };

    public static Result<T> Fail() =>
        new() { Succeeded = false };

    public static Result<T> Fail(string message) =>
        new() { Succeeded = false, Messages = [message] };

    public static Result<T> Fail(T data, string message) =>
        new()
        {
            Succeeded = false,
            Data = data,
            Messages = [message]
        };

    public static Result<T> Fail(List<string> messages) =>
        new() { Succeeded = false, Messages = messages };

    public static Result<T> Fail(T data, List<string> messages) =>
        new()
        {
            Succeeded = false,
            Data = data,
            Messages = messages
        };
}
