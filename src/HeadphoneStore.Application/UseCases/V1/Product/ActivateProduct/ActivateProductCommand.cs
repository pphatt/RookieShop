using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Product.ActivateProduct;

public sealed record ActivateProductCommand(Guid Id) : ICommand
{
}
