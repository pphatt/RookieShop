using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Category.ActivateCategory;

public class ActivateCategoryCommand : ICommand
{
    public Guid Id { get; set; }
}
