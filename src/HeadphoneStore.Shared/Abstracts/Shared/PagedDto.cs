namespace HeadphoneStore.Shared.Abstracts.Shared;

public class PagedDto
{
    public string? SearchTerm { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
