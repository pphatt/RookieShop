using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;

public class CreateCategoryCommand : ICommand
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? Status { get; set; }
}
