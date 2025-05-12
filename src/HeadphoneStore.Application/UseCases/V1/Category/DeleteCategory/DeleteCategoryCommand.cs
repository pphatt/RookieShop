using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Category.DeleteCategory;

public sealed record DeleteCategoryCommand(Guid Id) : ICommand
{
}
