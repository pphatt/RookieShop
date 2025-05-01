using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Category.InactivateCategory;

public class InactivateCategoryCommand : ICommand
{
    public Guid Id { get; set; }
}
