using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Application.UseCases.V1.Product.DeleteProduct;

using Exceptions = Domain.Exceptions.Exceptions;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id);

        if (product is null)
            throw new Exceptions.Product.NotFound();

        if (product.IsDeleted)
            throw new Exceptions.Product.AlreadyDeleted();

        product.Delete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Delete product successfully.");
    }
}
