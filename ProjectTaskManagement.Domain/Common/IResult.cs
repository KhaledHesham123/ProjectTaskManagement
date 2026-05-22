namespace ProjectTaskManagement.Domain.Common;

public interface IResult
{
    bool Succeeded { get; }
    string? Error { get; }
}
