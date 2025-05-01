using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.Shared.Services.Category.Update;

public class UpdateCategoryRequestDto
{
    [FromRoute]
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string Slug { get; set; }
    public required string Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string Status { get; set; }
}
