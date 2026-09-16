namespace CleanTeeth.Application.Contracts.Common.Model;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize);
