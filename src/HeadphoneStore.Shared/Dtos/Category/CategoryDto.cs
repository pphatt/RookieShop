namespace HeadphoneStore.Shared.Dtos.Category;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    public CategoryDto? Parent { get; set; }
    public IEnumerable<CategoryDto>? SubCategories { get; set; }
}
