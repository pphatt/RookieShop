using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.Shared.Services.Brand.Update;

public class UpdateBrandRequestDto
{
    [FromRoute]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
