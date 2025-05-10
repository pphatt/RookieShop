using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;

public class CreateBrandCommand : ICommand
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Status { get; set; }
}
