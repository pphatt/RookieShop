using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

namespace HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;

using Brand = Domain.Aggregates.Brands.Entities.Brand;
using Exceptions = Domain.Exceptions.Exceptions;

public class CreateBrandCommandHandler : ICommandHandler<CreateBrandCommand>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public CreateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var duplicateName = _brandRepository.FindByCondition(x => x.Name == request.Name).FirstOrDefault();

        if (duplicateName is not null)
            throw new Exceptions.Brand.DuplicateName();

        var isSlugAlreadyExisted = await _brandRepository.IsSlugAlreadyExisted(request.Slug);

        if (isSlugAlreadyExisted)
            throw new Exceptions.Brand.SlugExists();

        Enum.TryParse<EntityStatus>(request.Status, true, out var status);

        var category = Brand.Create(
            name: request.Name,
            slug: request.Slug!,
            description: request.Description,
            status: status
        );

        _brandRepository.Add(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("Brands", cancellationToken);

        return Result.Success("Create new brand successfully.");
    }
}
