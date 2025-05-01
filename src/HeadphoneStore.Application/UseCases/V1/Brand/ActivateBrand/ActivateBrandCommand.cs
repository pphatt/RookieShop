using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.ActiveBrand;

public class ActivateBrandCommand : ICommand
{
    public Guid Id { get; set; }
}
