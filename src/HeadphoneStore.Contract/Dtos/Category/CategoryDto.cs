namespace HeadphoneStore.Contract.Dtos.Category;

public class CategoryDtoBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public class CategoryDto : CategoryDtoBase
{
    public CategoryDtoBase? Parent { get; set; }
    public IEnumerable<CategoryDtoBase>? Children { get; set; }
}
