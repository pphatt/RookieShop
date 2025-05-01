namespace HeadphoneStore.Shared.Services.Brand.Create;

public class CreateBrandRequestDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
}
