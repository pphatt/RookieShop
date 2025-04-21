namespace HeadphoneStore.Contract.Services.Brand.Create;

public class CreateBrandRequestDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
