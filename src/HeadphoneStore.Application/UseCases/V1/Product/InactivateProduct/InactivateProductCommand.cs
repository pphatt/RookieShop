using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Product.InactivateProduct;

public sealed record InactivateProductCommand(Guid Id) : ICommand
{
}
