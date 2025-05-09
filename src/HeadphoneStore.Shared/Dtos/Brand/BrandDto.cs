namespace HeadphoneStore.Shared.Dtos.Brand;

public class BrandDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
}
