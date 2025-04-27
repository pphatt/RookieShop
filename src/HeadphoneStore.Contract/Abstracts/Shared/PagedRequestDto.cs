namespace HeadphoneStore.Contract.Abstracts.Shared;

public class PagedRequestDto
{
    public string? SearchTerm { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
