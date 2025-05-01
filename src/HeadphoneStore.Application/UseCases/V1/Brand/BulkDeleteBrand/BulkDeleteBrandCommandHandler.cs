using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Application.UseCases.V1.Brand.BulkDeleteBrand;

using Exceptions = Domain.Exceptions.Exceptions;

public class BulkDeleteBrandCommandHandler : ICommandHandler<BulkDeleteBrandCommand>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BulkDeleteBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
    {
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(BulkDeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brandIds = request.Ids;
        var successfullyDeletedItems = new List<Guid>();

        foreach (var id in brandIds)
        {
            var brand = await _brandRepository.FindByIdAsync(id);

            if (brand is null)
                throw new Exceptions.Brand.NotFound();

            if (brand.IsDeleted)
                throw new Exceptions.Brand.AlreadyDeleted();

            brand.Delete();

            successfullyDeletedItems.Add(id);
        }

        if (successfullyDeletedItems.Count < 0)
            return Result.Success("Nothing was deleted.");

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(messages: new List<string>
        {
            $"Successfully deleted {successfullyDeletedItems.Count} academic years.",
            "Each item is available for recovery."
        });
    }
}
