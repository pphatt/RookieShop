namespace HeadphoneStore.Contract.Services.Category.Create;

public class CreateCategoryRequestDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
}
