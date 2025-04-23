using HeadphoneStore.Application.Abstracts.Interface.Repositories;
using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;

namespace HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;

using Brand = Domain.Aggregates.Products.Entities.Brand;
using Exceptions = Domain.Exceptions.Exceptions;

public class CreateBrandCommandHandler : ICommandHandler<CreateBrandCommand>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
    {
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var duplicateName = _brandRepository.FindByCondition(x => x.Name == request.Name).FirstOrDefault();

        if (duplicateName is not null)
            throw new Exceptions.Brand.DuplicateName();

        var category = Brand.Create(
            name: request.Name,
            description: request.Description,
            createdBy: request.CreatedBy
        );

        _brandRepository.Add(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Create new brand successfully.");
    }
}
