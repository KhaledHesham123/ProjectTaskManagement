namespace ProjectTaskManagement.Application.Common.Models;

public class PaginationResponse<T>
{
    public int Count { get; set; }
    public List<T> Data { get; set; } = [];
}
