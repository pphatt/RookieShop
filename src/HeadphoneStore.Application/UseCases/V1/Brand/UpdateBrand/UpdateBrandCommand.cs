using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.UpdateBrand;

public sealed record UpdateBrandCommand(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string Status) : ICommand
{
}
