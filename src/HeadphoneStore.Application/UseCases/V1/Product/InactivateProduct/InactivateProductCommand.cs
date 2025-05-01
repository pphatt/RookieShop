using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Product.InactivateProduct;

public class InactivateProductCommand : ICommand
{
    public Guid Id { get; set; }
}
