using HeadphoneStore.Contract.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Category.DeleteCategory;

public class DeleteCategoryCommand : ICommand
{
    public Guid CategoryId { get; set; }
}
