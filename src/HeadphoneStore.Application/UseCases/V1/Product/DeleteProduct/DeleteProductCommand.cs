using HeadphoneStore.Shared.Abstracts.Commands;
namespace HeadphoneStore.Application.UseCases.V1.Product.DeleteProduct;

public class DeleteProductCommand : ICommand
{
    public Guid Id { get; set; }
}
