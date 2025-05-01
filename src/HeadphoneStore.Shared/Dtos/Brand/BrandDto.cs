namespace HeadphoneStore.Shared.Dtos.Brand;

public class BrandDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
}
