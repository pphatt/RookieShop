using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Product.ActivateProduct;

public class ActivateProductCommand : ICommand
{
    public Guid Id { get; set; }
}
