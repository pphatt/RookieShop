using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;

public class CreateBrandCommand : ICommand
{
    public string Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public Guid CreatedBy { get; set; }
}
