namespace ProjectTaskManagement.Domain.Common;

public interface IResult
{
    bool Succeeded { get; }
    List<string> Messages { get; }
}
