using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;

public sealed record CreateBrandCommand(
    string Name,
    string? Slug,
    string? Description,
    string? Status) : ICommand
{
}
