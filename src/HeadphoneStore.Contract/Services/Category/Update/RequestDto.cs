using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.Contract.Services.Category.Update;

public class UpdateCategoryRequestDto
{
    [FromRoute]
    public required Guid CategoryId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
}
