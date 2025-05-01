using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Application.UseCases.V1.Product.ActivateProduct;

using Exceptions = Domain.Exceptions.Exceptions;

public class ActivateProductCommandHandler : ICommandHandler<ActivateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public ActivateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id);

        if (product is null)
            throw new Exceptions.Product.NotFound();

        if (product.IsDeleted)
            throw new Exceptions.Product.AlreadyDeleted();

        if (product.Status == EntityStatus.Active)
            throw new Exceptions.Product.AlreadyActive();

        product.Activate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Activate product successfully.");
    }
}
