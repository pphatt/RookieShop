using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Domain.Exceptions;

namespace HeadphoneStore.Application.UseCases.V1.Products.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Create Product");
        return Result.Success();
    }
}
