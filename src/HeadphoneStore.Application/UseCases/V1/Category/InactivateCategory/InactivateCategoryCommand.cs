using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Category.InactivateCategory;

public sealed record InactivateCategoryCommand(Guid Id) : ICommand
{
}
