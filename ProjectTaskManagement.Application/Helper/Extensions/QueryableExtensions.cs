using ProjectTaskManagement.Application.Common.Models;

namespace ProjectTaskManagement.Application.Helper.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, Pagination pagination) =>
        query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize);
}
