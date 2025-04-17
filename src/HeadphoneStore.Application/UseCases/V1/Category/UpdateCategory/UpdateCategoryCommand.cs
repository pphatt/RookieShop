using HeadphoneStore.Contract.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Category.UpdateCategory;

public class UpdateCategoryCommand : ICommand
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Guid UpdatedBy { get; set; }
}
