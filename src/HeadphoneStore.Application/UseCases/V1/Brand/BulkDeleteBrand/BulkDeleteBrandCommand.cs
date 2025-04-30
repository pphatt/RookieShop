using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.BulkDeleteBrand;

public class BulkDeleteBrandCommand : ICommand
{
    public List<Guid> Ids { get; set; }
}
