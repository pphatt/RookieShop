using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Shared.Abstracts.Shared;

public class PagedResult<T>
{
    private const int MaxPageSize = 100;
    private const int DefaultPageSize = 10;
    private const int DefaultPageIndex = 1;

    [JsonConstructor]
    public PagedResult(List<T> items, int pageIndex, int pageSize, int totalCount)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public List<T> Items { get; set; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPage => (int)Math.Ceiling((double)TotalCount / PageSize);
    public int FirstRowOnPage => (PageIndex - 1) * PageSize + 1;
    public int LastRowOnPage => Math.Min(PageIndex * PageSize, TotalCount);
    public bool HasNextPage => PageIndex * PageSize < TotalCount;
    public bool HasPreviousPage => PageIndex > 1;

    public static async Task<PagedResult<T>> InitializeAsync(IQueryable<T> query, int pageIndex, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(query);

        pageIndex = Math.Max(pageIndex, DefaultPageIndex);

        pageSize = pageSize <= 0 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>(items, pageIndex, pageSize, totalCount);
    }
}
