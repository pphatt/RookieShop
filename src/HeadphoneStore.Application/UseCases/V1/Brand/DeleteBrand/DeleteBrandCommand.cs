using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;

public class DeleteBrandCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid UpdatedBy { get; set; }
}
