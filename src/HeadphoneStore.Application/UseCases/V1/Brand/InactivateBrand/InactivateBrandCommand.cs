using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.InactiveBrand;

public class InactivateBrandCommand : ICommand
{
    public Guid Id { get; set; }
}
