using HeadphoneStore.Contract.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Products.CreateProduct;

public class CreateProductCommand : ICommand
{
    public Guid Id { get; set; }
}
