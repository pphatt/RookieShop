using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;

public sealed record CreateCategoryCommand(
    string Name,
    string? Slug,
    string Description,
    Guid? ParentCategoryId,
    string? Status) : ICommand
{
}
