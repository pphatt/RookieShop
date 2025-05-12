using HeadphoneStore.Shared.Abstracts.Commands;
namespace HeadphoneStore.Application.UseCases.V1.Product.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id) : ICommand
{
}
