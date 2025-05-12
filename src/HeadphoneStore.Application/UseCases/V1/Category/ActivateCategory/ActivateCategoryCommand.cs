using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Category.ActivateCategory;

public sealed record ActivateCategoryCommand(Guid Id) : ICommand
{
}
